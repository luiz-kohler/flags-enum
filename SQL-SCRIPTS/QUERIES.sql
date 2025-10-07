-- Users who are ONLY Hirers 
SELECT ID, NAME, ROLES
FROM USERS
WHERE ROLES = 2;

-- Users who are ONLY Passengers
SELECT ID, NAME, ROLES
FROM USERS
WHERE ROLES = 4;

-- Users who are ONLY Financial Managers
SELECT ID, NAME, ROLES
FROM USERS
WHERE ROLES = 8;

---------------------------------------

-- Users with Hirer role (value 2)
SELECT ID, NAME, ROLES 
FROM USERS 
WHERE ROLES & 2 = 2;

-- Users with Passenger role (value 4)  
SELECT ID, NAME, ROLES
FROM USERS
WHERE ROLES & 4 = 4;

-- Users with Financial Manager role (value 8)
SELECT ID, NAME, ROLES
FROM USERS
WHERE ROLES & 8 = 8;

---------------------------------------

-- Users  Hirer AND Passenger (value 6)
SELECT ID, NAME, ROLES
FROM USERS
WHERE ROLES & 6 = 6;  -- 2 (Hirer) + 4 (Passenger) = 6

-- Users Hirer AND Financial Manager (value 10)
SELECT ID, NAME, ROLES
FROM USERS
WHERE ROLES & 10 = 10;  -- 2 (Hirer) + 8 (Financial) = 10

-- Users Passenger AND Financial Manager (value 12)
SELECT ID, NAME, ROLES
FROM USERS
WHERE ROLES & 12 = 12;  -- 4 (Passenger) + 8 (Financial) = 12

---------------------------------------

-- Users NOT Hirers
SELECT ID, NAME, ROLES
FROM USERS
WHERE ROLES & 2 = 0;

-- Users NOT Financial Managers
SELECT ID, NAME, ROLES
FROM USERS
WHERE ROLES & 8 = 0;

-- Users NO roles
SELECT ID, NAME, ROLES
FROM USERS
WHERE ROLES = 0;