-- Migration script to rename Strategies table to StrategyCaches
-- Run this on your existing database to update the table name

USE BeyondBotDB;

-- Rename the table
RENAME TABLE Strategies TO StrategyCaches;