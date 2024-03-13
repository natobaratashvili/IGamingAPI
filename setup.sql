CREATE SCHEMA db_igaming;
CREATE TABLE db_igaming.users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    email VARCHAR(255) NOT NULL UNIQUE,
    username VARCHAR(50) NOT NULL UNIQUE,
    guid VARCHAR(45) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL, 
    create_date_utc datetime DEFAULT now()
);
CREATE INDEX idx_id ON db_igaming.users (id);
CREATE INDEX idx_username ON db_igaming.users (username);

		
CREATE TABLE db_igaming.wallet (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT,
    FOREIGN KEY (user_id) REFERENCES db_igaming.users(id),
    balance decimal(10,2) default 1000,
    create_date_utc datetime DEFAULT now(),
    update_date_utc datetime DEFAULT now()
);

CREATE INDEX idx_id_wallet ON db_igaming.wallet (id);
CREATE INDEX idx_user_id ON db_igaming.wallet (user_id);

CREATE TABLE db_igaming.bets (
    id INT AUTO_INCREMENT,
    user_id INT,
    amount decimal(10,2) not null,
    details VARCHAR(500) DEFAULT NULL,
    create_date_utc datetime DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY(id,create_date_utc)
)
 PARTITION BY RANGE COLUMNS (create_date_utc) (
        PARTITION p202402 VALUES LESS THAN ('2024-04-01'),
        PARTITION p202403 VALUES LESS THAN ('2024-05-01'),
        PARTITION p202404 VALUES LESS THAN ('2024-06-01'),
        PARTITION pmax VALUES LESS THAN MAXVALUE
    );

DELIMITER //

CREATE TRIGGER db_igaming.before_insert_bets
BEFORE INSERT ON db_igaming.bets
FOR EACH ROW
BEGIN
    DECLARE user_exists INT;

    SELECT COUNT(*) INTO user_exists FROM db_igaming.users WHERE id = NEW.user_id;
    IF user_exists = 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'User does not exist in the users table';
    END IF;
END//

DELIMITER ;

CREATE INDEX idx_id_bets ON db_igaming.bets (id);
CREATE INDEX idx_user_id_bets ON db_igaming.bets (user_id);

CREATE USER 'IGamingApi'@'localhost' IDENTIFIED BY 'yaw6pN3V';
GRANT ALL PRIVILEGES ON db_igaming.* TO 'IGamingApi'@'localhost';