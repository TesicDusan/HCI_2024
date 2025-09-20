# 🎬 Cinema Management App (WPF, C#)

A desktop application built with **WPF (.NET, C#)** and **MySQL** for managing a cinema.  
The app allows users to browse movie showings, book tickets, manage orders, view receipts, and customize preferences (language & theme).

---

## ✨ Features

- 📅 **Movie Showings**
  - Browse current and upcoming movie showings
  - View posters, start times, and occupancy rates
  - Book tickets by selecting seats

- 🎟️ **Ticket Ordering**
  - Add tickets to an order
  - View and manage selected seats
  - Save orders to database

- 🧾 **Receipts**
  - Each completed order generates a `.txt` receipt
  - Browse past receipts in-app
  - Click a receipt to open it directly

- ⚙️ **User Options**
  - Change **language** (English / Serbian)
  - Change **theme** (Light / Dark)
  - Change **PIN (password)** through a secure dialog
  - Preferences are stored per user in the database

- 🔑 **Admin Options**
  - Add and remove movies, showings, snacks, drinks, and users
  - Make users admins
  - View all receipts

---

## 🛠️ Technologies

- **.NET WPF (C#)** for UI
- **MaterialDesignInXAML Toolkit** for modern UI components
- **MySQL** database for storing users, showings, and orders
- **MVVM pattern** with data binding
- **ObservableCollections & INotifyPropertyChanged** for dynamic UI updates

---

## 📂 Project Structure

HCI_2024/
│
├── Models/ # Data models (OrderItem, MovieShowing, Seat, etc.)
├── ViewModels/ # ViewModels (ShowingsViewModel, OptionsViewModel, etc.)
├── Views/ # Pages & dialogs (MoviesPage, OptionsPage, InputDialog, etc.)
├── Services/ # DatabaseHelper and other services
├── Resources/ # Localization, themes, and images
├── Database/ # SQL schema and seed data
├── Properties/ # Project properties
├── App.config # Application configuration
├── App.xaml # Application entry point
└── README.md # This file

---

## 🚀 Getting Started

### 1. Requirements
- Visual Studio 2022 or newer
- .NET 6 or newer
- MySQL server installed

### 2. Setup
1. Clone the repository
2. Import the provided MySQL schema
3. Update the **connection string** in `DatabaseHelper.cs`
4. Build and run the project in Visual Studio

### 3. Default Settings
- Default language: `en`
- Default theme: `light`

---

## 📖 Usage

- Go to **Movies Page** → select showings and book tickets
- Go to **Snacks Page** → add snacks to your order
- Go to **Drinks Page** → add drinks to your order
- Go to **Options Page** → change language, theme, or PIN
- Receipts are automatically saved in `.txt` format and accessible in the app
