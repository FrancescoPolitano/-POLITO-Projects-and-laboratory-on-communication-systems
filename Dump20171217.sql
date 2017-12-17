-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: paldb
-- ------------------------------------------------------
-- Server version	5.7.20-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `accesses`
--

DROP TABLE IF EXISTS `accesses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `accesses` (
  `Id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `IdEmployee` int(5) unsigned NOT NULL,
  `IdLocal` char(8) NOT NULL,
  `TimeS` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Result` set('true','false') NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IdEmployee` (`IdEmployee`),
  KEY `accesses_ibfk_2` (`IdLocal`),
  CONSTRAINT `accesses_ibfk_1` FOREIGN KEY (`IdEmployee`) REFERENCES `employes` (`SerialNumber`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `accesses_ibfk_2` FOREIGN KEY (`IdLocal`) REFERENCES `locals` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=120 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accesses`
--

LOCK TABLES `accesses` WRITE;
/*!40000 ALTER TABLE `accesses` DISABLE KEYS */;
/*!40000 ALTER TABLE `accesses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `auth`
--

DROP TABLE IF EXISTS `auth`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `auth` (
  `IdEmployee` int(5) unsigned NOT NULL,
  `Code` char(12) NOT NULL,
  PRIMARY KEY (`IdEmployee`),
  UNIQUE KEY `Code` (`Code`),
  CONSTRAINT `auth_ibfk_3` FOREIGN KEY (`IdEmployee`) REFERENCES `employes` (`SerialNumber`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `auth`
--

LOCK TABLES `auth` WRITE;
/*!40000 ALTER TABLE `auth` DISABLE KEYS */;
INSERT INTO `auth` VALUES (20087,'2imI9g70BNbG'),(20080,'2uEqCIDIc5fd'),(20094,'4vLhJ6yDVyYh'),(20078,'5FIaKaNMhKGc'),(20084,'8B0aHM2DK2bw'),(20096,'FjGM3Z7K51PH'),(20077,'fNIipXoirDIc'),(20082,'fwd7SlyZnEQH'),(20097,'L7KLLBOoEwp8'),(20093,'mJyA9Ifg27Ps'),(20083,'nA14SU7ZuZA0'),(20092,'NsHRAy1PCjb4'),(20090,'qnTdbg7bsqkU'),(20081,'sxs912yiOtwI'),(20091,'VPWni0xoY2lf'),(20086,'x0ZBgOLxWMjW'),(20085,'x8GHXxKmDu6J'),(20088,'XiFbmNvCXAhB'),(20089,'XJ2qStLszJ1Z'),(20079,'yNANbSNqOFHG'),(20095,'yywpgeMXq4JN');
/*!40000 ALTER TABLE `auth` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employes`
--

DROP TABLE IF EXISTS `employes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `employes` (
  `SerialNumber` int(5) unsigned NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL DEFAULT '0',
  `Surname` varchar(50) NOT NULL DEFAULT '0',
  `AuthGrade` enum('1','2','3') NOT NULL,
  `Expiration` datetime DEFAULT NULL,
  `Causal` varchar(140) DEFAULT NULL,
  PRIMARY KEY (`SerialNumber`)
) ENGINE=InnoDB AUTO_INCREMENT=20098 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employes`
--

LOCK TABLES `employes` WRITE;
/*!40000 ALTER TABLE `employes` DISABLE KEYS */;
INSERT INTO `employes` VALUES (20077,'Gianmaria','Tremigliozzi','3',NULL,NULL),(20078,'Cristiano','Palazzi','1',NULL,NULL),(20079,'didi','dodod','1',NULL,NULL),(20080,'pippo','ventuno','1',NULL,NULL),(20081,'antonio','verrilli','2',NULL,NULL),(20082,'gino','pinguino','1',NULL,NULL),(20083,'battone','battona','2',NULL,NULL),(20084,'gino','pinguino','1',NULL,NULL),(20085,'banana','banana','1',NULL,NULL),(20086,'banana','banana','1',NULL,NULL),(20087,'antonio',' pinguni','1',NULL,NULL),(20088,'banana','banana','1',NULL,NULL),(20089,'dwdwdwdw','111','1',NULL,NULL),(20090,'banana','banana','1',NULL,NULL),(20091,'banana','banana','1',NULL,NULL),(20092,'banana','banana','1',NULL,NULL),(20093,'banana','banana','1',NULL,NULL),(20094,'banana','banana','1',NULL,NULL),(20095,'banana','banana','1',NULL,NULL),(20096,'banana','banana','1',NULL,NULL),(20097,'qrcode','fotodeltizio','1',NULL,NULL);
/*!40000 ALTER TABLE `employes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `locals`
--

DROP TABLE IF EXISTS `locals`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `locals` (
  `Id` char(8) NOT NULL,
  `AuthGrade` set('1','2','3') NOT NULL,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `Name` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `locals`
--

LOCK TABLES `locals` WRITE;
/*!40000 ALTER TABLE `locals` DISABLE KEYS */;
INSERT INTO `locals` VALUES ('BOSSDESK','3','Ufficio CEO'),('CRISDESK','1','Ufficio Cristiano'),('MENSAUNO','1','Mensa ufficio'),('OFFICE4H','2','Ufficio contabilità'),('STOCKS00','1','Scarico merci');
/*!40000 ALTER TABLE `locals` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `photos`
--

DROP TABLE IF EXISTS `photos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `photos` (
  `IdEmployee` int(5) unsigned NOT NULL,
  `photo` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`IdEmployee`),
  CONSTRAINT `photos_ibfk_1` FOREIGN KEY (`IdEmployee`) REFERENCES `employes` (`SerialNumber`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `photos`
--

LOCK TABLES `photos` WRITE;
/*!40000 ALTER TABLE `photos` DISABLE KEYS */;
INSERT INTO `photos` VALUES (20077,'/images/profiles/20077.jpg'),(20078,'/images/profiles/20078.jpg'),(20079,'/images/profiles/20079.jpg'),(20080,'/images/profiles/20080.jpg'),(20081,'/images/profiles/20081.jpg'),(20082,'/images/profiles/20082.jpg'),(20083,'/images/profiles/20083.jpg'),(20084,'/images/profiles/20084.jpg'),(20085,'/images/profiles/20085.jpg'),(20086,'/images/profiles/20086.jpg'),(20087,'/images/profiles/20087.jpg'),(20088,'/images/profiles/20088.jpg'),(20089,'/images/profiles/20089.jpg'),(20090,'/images/profiles/20090.jpg'),(20091,'/images/profiles/20091.jpg'),(20092,'/images/profiles/20092.jpg'),(20093,'/images/profiles/20093.jpg'),(20094,'/images/profiles/20094.jpg'),(20095,'/images/profiles/20095.jpg'),(20096,'/images/profiles/20096.jpg'),(20097,'/images/profiles/20097.jpg');
/*!40000 ALTER TABLE `photos` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-12-17 13:28:26
