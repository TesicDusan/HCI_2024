CREATE DATABASE  IF NOT EXISTS `hci2024` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `hci2024`;
-- MySQL dump 10.13  Distrib 8.0.32, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: hci2024
-- ------------------------------------------------------
-- Server version	8.0.32

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `items`
--

DROP TABLE IF EXISTS `items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `items` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `Type` int NOT NULL,
  `PictureUrl` varchar(500) NOT NULL,
  `IsVisible` tinyint NOT NULL DEFAULT '1',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `items`
--

LOCK TABLES `items` WRITE;
/*!40000 ALTER TABLE `items` DISABLE KEYS */;
INSERT INTO `items` VALUES (1,'Popcorn',3.00,2,'https://atlas-content-cdn.pixelsquid.com/stock-images/popcorn-bucket-zeJmWv5-600.jpg',1),(2,'Soda',2.00,1,'https://media.istockphoto.com/id/1129679604/photo/isolated-soda-cup-with-straw.jpg?s=612x612&w=0&k=20&c=SgMKnwnsqr95jellGZoPK7GietHqL2T5R7UbQ2bJWSg=',1);
/*!40000 ALTER TABLE `items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `movies`
--

DROP TABLE IF EXISTS `movies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `movies` (
  `MovieId` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `PosterUrl` varchar(500) NOT NULL,
  `IsVisible` tinyint NOT NULL DEFAULT '1',
  PRIMARY KEY (`MovieId`),
  UNIQUE KEY `Name_UNIQUE` (`Name`),
  UNIQUE KEY `PosterUrl_UNIQUE` (`PosterUrl`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `movies`
--

LOCK TABLES `movies` WRITE;
/*!40000 ALTER TABLE `movies` DISABLE KEYS */;
INSERT INTO `movies` VALUES (1,'Inception','https://cdn11.bigcommerce.com/s-yzgoj/images/stencil/1280x1280/products/2919271/5944675/MOVEB46211__19379.1679590452.jpg',1);
/*!40000 ALTER TABLE `movies` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orderitems`
--

DROP TABLE IF EXISTS `orderitems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orderitems` (
  `OrderItemId` int NOT NULL AUTO_INCREMENT,
  `OrderId` int NOT NULL,
  `ItemType` int NOT NULL,
  `Name` varchar(45) NOT NULL,
  `Quantity` int NOT NULL DEFAULT '1',
  `Price` decimal(10,2) NOT NULL,
  `ShowingId` int DEFAULT NULL,
  `ItemId` int DEFAULT NULL,
  PRIMARY KEY (`OrderItemId`),
  KEY `OrderId_idx` (`OrderId`),
  KEY `fk_orderitems_items_idx` (`ItemId`),
  KEY `fk_orderitems_showings_idx` (`ShowingId`),
  CONSTRAINT `fk_orderitems_items` FOREIGN KEY (`ItemId`) REFERENCES `items` (`Id`) ON DELETE SET NULL,
  CONSTRAINT `fk_orderitems_showings` FOREIGN KEY (`ShowingId`) REFERENCES `showings` (`ShowingID`) ON DELETE SET NULL,
  CONSTRAINT `fk_oredritems_orders` FOREIGN KEY (`OrderId`) REFERENCES `orders` (`OrderId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orderitems`
--

LOCK TABLES `orderitems` WRITE;
/*!40000 ALTER TABLE `orderitems` DISABLE KEYS */;
INSERT INTO `orderitems` VALUES (1,1,2,'Inception 01/09/2025 19:00:00',2,10.00,1,NULL),(2,1,0,'Soda',2,2.00,NULL,2),(3,1,1,'Popcorn',2,3.00,NULL,1),(4,2,2,'Inception 01/09/2025 19:00:00',1,10.00,1,NULL),(5,3,2,'Inception 01/09/2025 19:00:00',4,10.00,1,NULL),(6,3,0,'Soda',4,2.00,NULL,2),(7,3,1,'Popcorn',2,3.00,NULL,1),(8,4,2,'Inception 01/09/2025 19:00:00',1,10.00,1,NULL),(9,5,2,'Inception 01/09/2025 22:00:00',4,10.00,2,NULL),(10,5,0,'Soda',5,2.00,NULL,2),(11,5,1,'Popcorn',4,3.00,NULL,1);
/*!40000 ALTER TABLE `orderitems` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orders` (
  `OrderId` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `TotalPrice` decimal(10,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`OrderId`),
  KEY `UserId_idx` (`UserId`),
  CONSTRAINT `UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`userId`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (1,1001,30.00),(2,1001,10.00),(3,1001,54.00),(4,1001,10.00),(5,1000,62.00);
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `seatreservations`
--

DROP TABLE IF EXISTS `seatreservations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `seatreservations` (
  `ReservationId` int NOT NULL AUTO_INCREMENT,
  `ShowingId` int NOT NULL,
  `SeatId` int NOT NULL,
  PRIMARY KEY (`ReservationId`),
  UNIQUE KEY `uq_showing_seat` (`ShowingId`,`SeatId`),
  KEY `ShowingId_idx` (`ShowingId`),
  KEY `SeatId_idx` (`SeatId`),
  CONSTRAINT `SeatId` FOREIGN KEY (`SeatId`) REFERENCES `seats` (`SeatId`),
  CONSTRAINT `ShowingId` FOREIGN KEY (`ShowingId`) REFERENCES `showings` (`ShowingID`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `seatreservations`
--

LOCK TABLES `seatreservations` WRITE;
/*!40000 ALTER TABLE `seatreservations` DISABLE KEYS */;
INSERT INTO `seatreservations` VALUES (3,1,25),(4,1,26),(5,1,35),(10,1,37),(6,1,44),(7,1,45),(8,1,46),(9,1,47),(11,2,43),(12,2,44),(13,2,45),(14,2,46);
/*!40000 ALTER TABLE `seatreservations` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `seats`
--

DROP TABLE IF EXISTS `seats`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `seats` (
  `SeatId` int NOT NULL AUTO_INCREMENT,
  `SeatNumber` int NOT NULL,
  PRIMARY KEY (`SeatId`),
  UNIQUE KEY `SeatNumber_UNIQUE` (`SeatNumber`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `seats`
--

LOCK TABLES `seats` WRITE;
/*!40000 ALTER TABLE `seats` DISABLE KEYS */;
INSERT INTO `seats` VALUES (1,1),(2,2),(3,3),(4,4),(5,5),(6,6),(7,7),(8,8),(9,9),(10,10),(11,11),(12,12),(13,13),(14,14),(15,15),(16,16),(17,17),(18,18),(19,19),(20,20),(21,21),(22,22),(23,23),(24,24),(25,25),(26,26),(27,27),(28,28),(29,29),(30,30),(31,31),(32,32),(33,33),(34,34),(35,35),(36,36),(37,37),(38,38),(39,39),(40,40),(41,41),(42,42),(43,43),(44,44),(45,45),(46,46),(47,47),(48,48),(49,49),(50,50);
/*!40000 ALTER TABLE `seats` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `showings`
--

DROP TABLE IF EXISTS `showings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `showings` (
  `ShowingID` int NOT NULL AUTO_INCREMENT,
  `MovieId` int NOT NULL,
  `StartTime` datetime NOT NULL,
  `TicketPrice` decimal(10,2) NOT NULL DEFAULT '10.00',
  `IsVisible` tinyint NOT NULL DEFAULT '1',
  PRIMARY KEY (`ShowingID`),
  KEY `MovieId_idx` (`MovieId`),
  CONSTRAINT `MovieId` FOREIGN KEY (`MovieId`) REFERENCES `movies` (`MovieId`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `showings`
--

LOCK TABLES `showings` WRITE;
/*!40000 ALTER TABLE `showings` DISABLE KEYS */;
INSERT INTO `showings` VALUES (1,1,'2025-09-11 19:00:00',10.00,1),(2,1,'2025-09-01 22:00:00',10.00,1);
/*!40000 ALTER TABLE `showings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `userId` int NOT NULL,
  `userPin` int NOT NULL,
  `languagePreference` varchar(45) NOT NULL DEFAULT 'en',
  `themePreference` varchar(45) NOT NULL DEFAULT 'LightTheme',
  `admin` tinyint NOT NULL DEFAULT '0',
  `visible` tinyint NOT NULL DEFAULT '1',
  PRIMARY KEY (`userId`),
  UNIQUE KEY `userId_UNIQUE` (`userId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1000,1111,'en','DarkTheme',1,1),(1001,1001,'en','LightTheme',0,1);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'hci2024'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-09-20 23:45:42
