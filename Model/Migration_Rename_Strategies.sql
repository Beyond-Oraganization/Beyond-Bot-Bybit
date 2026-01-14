-- Migration script to rename StrategyCaches table to IndicatorCaches
-- Run this on your existing database to update the table name

USE BeyondBotDB;

-- Rename the table
RENAME TABLE StrategyCaches TO IndicatorCaches;