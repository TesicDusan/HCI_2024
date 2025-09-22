using HCI_2024.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO.Packaging;
using System.Windows.Documents;

namespace HCI_2024.Services
{
    public static class DatabaseHelper
    {
        private static readonly string ConnectionString = "Server=127.0.0.1;Database=hci2024;User Id=root;Password=753fgf357";

        public static bool IsUserValid(string userId, string pin)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM users WHERE userId = @UserID AND userPin = @Pin AND visible = 1";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        command.Parameters.AddWithValue("@Pin", pin);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count == 1;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Database error: {e.Message}");
                    return false;
                }
            }
        }

        public static ObservableCollection<Seat> LoadSeats(int showingId)
        {
            ObservableCollection<Seat> seats = new ObservableCollection<Seat>();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                var seatCommand = new MySqlCommand("SELECT SeatId, SeatNumber FROM seats ORDER BY SeatNumber", connection);
                var seatReader = seatCommand.ExecuteReader();

                var allSeats = new List<Models.Seat>();
                while (seatReader.Read())
                {
                    allSeats.Add(new Models.Seat
                    {
                        SeatId = seatReader.GetInt32("SeatId"),
                        SeatNumber = seatReader.GetInt32("SeatNumber"),
                        IsAvailable = true,
                        IsSelected = false
                    });
                }
                seatReader.Close();

                var reservedCommand = new MySqlCommand("SELECT SeatId FROM seatreservations WHERE ShowingId = @ShowingId", connection);
                reservedCommand.Parameters.AddWithValue("@ShowingId", showingId);

                var reservedSeats = new HashSet<int>();
                var reservedReader = reservedCommand.ExecuteReader();
                while (reservedReader.Read())
                {
                    reservedSeats.Add(reservedReader.GetInt32("SeatId"));
                }
                reservedReader.Close();

                foreach (var seat in allSeats)
                {
                    if (reservedSeats.Contains(seat.SeatId))
                    {
                        seat.IsAvailable = false;
                    }
                    seats.Add(seat);
                }
            }

            return seats;
        }

        public static ObservableCollection<MovieShowing> LoadShowings(DateTime dateTime)
        {
            ObservableCollection<MovieShowing> Showings = new ObservableCollection<MovieShowing>();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT
                        s.ShowingId,
                        s.MovieId,
                        m.Name AS MovieName,
                        m.PosterUrl,
                        s.StartTime,
                        s.TicketPrice,
                        s.IsVisible,
                        COUNT(r.SeatId) / 50.0 AS OccupancyRate
                    FROM showings s
                    JOIN movies m ON s.MovieId = m.MovieId
                    LEFT JOIN seatreservations r ON s.ShowingId = r.ShowingId
                    GROUP BY s.ShowingId, s.MovieId, m.Name, m.PosterUrl, s.StartTime, s.TicketPrice
                    HAVING DATE(s.StartTime) = @Date AND s.IsVisible = 1";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Date", dateTime.Date);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var showing = new Models.MovieShowing
                            {
                                ShowingId = reader.GetInt32("ShowingId"),
                                MovieId = reader.GetInt32("MovieId"),
                                MovieName = reader.GetString("MovieName"),
                                PosterUrl = reader.GetString("PosterUrl"),
                                StartTime = reader.GetDateTime("StartTime"),
                                TicketPrice = reader.GetDecimal("TicketPrice"),
                                OccupancyRate = reader.GetDouble("OccupancyRate")
                            };
                            Showings.Add(showing);
                        }
                    }
                }
            }

            return Showings;
        }

        public static ObservableCollection<Item> LoadItems(ItemType type)
        {
            ObservableCollection<Item> items = new ObservableCollection<Item>();
            int typeId;
            if (type == ItemType.Drink)
                typeId = 1;
            else if (type == ItemType.Snack)
                typeId = 2;
            else
                throw new ArgumentException("Invalid item type");


            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM items WHERE Type = @Type AND IsVisible = 1";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Type", typeId.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new Item
                            {
                                ItemId = reader.GetInt32("Id"),
                                ItemName = reader.GetString("Name"),
                                PictureUrl = reader.GetString("PictureUrl"),
                                Price = reader.GetDecimal("Price"),
                                Type = type
                            };
                            items.Add(item);
                        }
                    }
                }
            }
            return items;
        }

        public static ObservableCollection<string> LoadMovies()
        {
            ObservableCollection<string> movies = new ObservableCollection<string>();
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT Name FROM movies WHERE IsVisible = 1";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            movies.Add(reader.GetString("Name"));
                        }
                    }
                }
            }
            return movies;
        }

        public static int SaveOrderToDatabase(ObservableCollection<OrderItem> orderItems, string userId, decimal totalPrice)
        {

            int orderId = -1;

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    string orderSql = "INSERT INTO orders (UserId, TotalPrice) VALUES (@user, @total); SELECT LAST_INSERT_ID();";
                    using (var cmd = new MySqlCommand(orderSql, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@user", int.Parse(userId));
                        cmd.Parameters.AddWithValue("@total", totalPrice);

                        orderId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    string itemSql = "INSERT INTO orderitems (OrderId, ItemType, Name, Quantity, Price, ShowingId, ItemId) " +
                                     "VALUES (@orderId, @type, @name, @quantity, @price, @showingId, @itemId);";
                    foreach (var orderItem in orderItems)
                    {
                        using (var cmd = new MySqlCommand(itemSql, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@orderId", orderId);
                            cmd.Parameters.AddWithValue("@type", (int)orderItem.Type);
                            cmd.Parameters.AddWithValue("@name", orderItem.Name);
                            cmd.Parameters.AddWithValue("@quantity", orderItem.Quantity);
                            cmd.Parameters.AddWithValue("@price", orderItem.Price);
                            cmd.Parameters.AddWithValue("@showingId", (object?)orderItem.ShowingId ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@itemId", (object?)orderItem.ItemId ?? DBNull.Value);
                            cmd.ExecuteNonQuery();
                        }

                        if (orderItem.Type == ItemType.Ticket && orderItem.ShowingId.HasValue)
                        {
                            string seatSql = "INSERT INTO seatreservations (ShowingId, SeatId) VALUES (@showingId, @seatId);";
                            for (int i = 0; i < orderItem.Quantity; i++)
                            {
                                using (var cmd = new MySqlCommand(seatSql, connection, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@showingId", orderItem.ShowingId.Value);
                                    cmd.Parameters.AddWithValue("@seatId", orderItem.SeatIds.ElementAt(i));
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    transaction.Commit();
                }
            }

            return orderId;
        }

        public static void SaveLanguagePreference(string userId, string language)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE users SET languagePreference = @Language WHERE userId = @UserId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Language", language);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static string GetLanguagePreference(string userId)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT languagePreference FROM users WHERE userId = @UserId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    var result = command.ExecuteScalar();
                    return result != null ? result.ToString().Trim() : "en";
                }
            }
        }

        public static void SaveThemePreference(string userId, string theme)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE users SET themePreference = @Theme WHERE userId = @UserId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Theme", theme);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static string GetThemePreference(string userId)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT themePreference FROM users WHERE userId = @UserId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    var result = command.ExecuteScalar();
                    return result != null ? result.ToString().Trim() : "LightTheme";
                }
            }
        }

        public static void ChangePin(string userId, string newPin)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE users SET userPin = @NewPin WHERE userId = @UserId";
                using (var command = new MySqlCommand(query, connection))
                {
                    if (int.TryParse(newPin, out int pinValue))
                    {
                        command.Parameters.AddWithValue("@NewPin", pinValue);
                        command.Parameters.AddWithValue("@UserId", int.Parse(userId));
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void AddUser(string userId, string pin)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO users (userId, userPin, languagePreference, themePreference) VALUES (@UserId, @Pin, 'en', 'LightTheme')";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Pin", pin);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteUser(string userId)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE users SET visible = 0 WHERE userId = @UserId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static bool IsAdmin(string userId)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT admin FROM users WHERE userId = @UserId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    var result = command.ExecuteScalar();
                    return result != null && Convert.ToBoolean(result);
                }
            }
        }

        public static void MakeAdmin(string userId)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE users SET admin = 1 WHERE userId = @UserId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static bool UserExists(string userId)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM users WHERE userId = @UserId AND visible = 1";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public static void AddMovie(string name, string posterUrl)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO movies (Name, PosterUrl) VALUES (@Name, @PosterUrl)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@PosterUrl", posterUrl);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void AddShowing(int movieId, DateTime startTime, decimal ticketPrice)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO showings (MovieId, StartTime, TicketPrice) VALUES (@MovieId, @StartTime, @TicketPrice)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MovieId", movieId);
                    command.Parameters.AddWithValue("@StartTime", startTime);
                    command.Parameters.AddWithValue("@TicketPrice", ticketPrice);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void RemoveShowing(int showingId)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE showings SET IsVisible = 0 WHERE ShowingId = @ShowingId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ShowingId", showingId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void AddItem(string name, string pictureUrl, decimal price, ItemType type)
        {
            int typeId = type == ItemType.Drink ? 1 : type == ItemType.Snack ? 2 : throw new ArgumentException("Invalid item type");
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO items (Name, PictureUrl, Price, Type) VALUES (@Name, @PictureUrl, @Price, @Type)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@PictureUrl", pictureUrl);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@Type", typeId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void RemoveItem(string itemName)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE items SET IsVisible = 0 WHERE Name = @ItemName";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemName", itemName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteMovie(string movieName)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE movies SET IsVisible = 0 WHERE Name = @MovieName";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MovieName", movieName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static int GetMovieId(string movieName)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT MovieId FROM movies WHERE Name = @MovieName";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MovieName", movieName);
                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }
    }
}
