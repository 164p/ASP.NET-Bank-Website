# ASP.NET Bank Website

 ---SETUP---<br>
1.Prepare new MySQL schema name "bank"<br>

2.Execute this SQL command<br>

CREATE TABLE Users (<br>
    Id INT PRIMARY KEY AUTO_INCREMENT,<br>
    Username VARCHAR(50) NOT NULL UNIQUE,<br>
    Password VARCHAR(100) NOT NULL,<br>
    Balance DECIMAL(18, 2) NOT NULL DEFAULT 0.00<br>
);<br>

CREATE TABLE Transactions (<br>
    Id INT PRIMARY KEY AUTO_INCREMENT,<br>
    UserId INT NOT NULL,<br>
    ActionType VARCHAR(50) NOT NULL,<br>
    FromUserId INT NULL,<br>
    Amount DECIMAL(18, 2) NOT NULL,<br>
    Remain DECIMAL(18, 2) NOT NULL,<br>
    Date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,<br>
    FOREIGN KEY (UserId) REFERENCES Users(Id),<br>
    FOREIGN KEY (FromUserId) REFERENCES Users(Id)<br>
);

INSERT INTO users (username, Password, Balance)<br>
VALUES ('michael', 123456, 0);<br>

INSERT INTO users (username, Password, Balance)<br>
VALUES ('johnny', 123456, 0);<br>

INSERT INTO users (username, Password, Balance)<br>
VALUES ('lilly', 123456, 0);<br>

INSERT INTO users (username, Password, Balance)<br>
VALUES ('jacob', 123456, 0);<br>

3.edit your connection string in appsetting.json for your database<br>

4.run the project and select one user to login then test all feature!!<br>

---FEATURE---<br>
1.Deposit<br>
2.Withdraw<br>
3.Transfer<br>
4.Transaction view (Received/Transfer)<br>
