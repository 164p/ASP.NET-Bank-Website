# ASP.NET Bank Website

 ---SETUP---
1.Prepare new MySQL schema name "bank"
2.Execute this SQL command
CREATE TABLE Users (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(100) NOT NULL,
    Balance DECIMAL(18, 2) NOT NULL DEFAULT 0.00
);

CREATE TABLE Transactions (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT NOT NULL,
    ActionType VARCHAR(50) NOT NULL,
    FromUserId INT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    Remain DECIMAL(18, 2) NOT NULL,
    Date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (FromUserId) REFERENCES Users(Id)
);

INSERT INTO users (username, Password, Balance)
VALUES ('michael', 123456, 0);

INSERT INTO users (username, Password, Balance)
VALUES ('johnny', 123456, 0);

INSERT INTO users (username, Password, Balance)
VALUES ('lilly', 123456, 0);

INSERT INTO users (username, Password, Balance)
VALUES ('jacob', 123456, 0);

3.edit your connection string in appsetting.json for your database

4.run the project and select one user to login then test all feature!!

---FEATURE---
1.Deposit
2.Withdraw
3.Transfer
4.Transaction view (Received/Transfer)
