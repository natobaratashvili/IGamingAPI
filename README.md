# IGamingAPI
This API simulate basic functionality for managing user accounts and placing bets in an iGaming platform.

# Project Overview
Upon receiving the terms of reference, the primary objective was to thoroughly understand the key requirements to effectively focus on them.

## Application Structure Selection
Selecting the appropriate structure for the application posed the initial challenge. Given the absence of extensive business logic, opting for the classic clean architecture structure would have unnecessarily complicated the project. Consequently, a simplified approach was adopted, with a particular emphasis on modularizing the infrastructure to facilitate seamless addition of future functionalities.

## Database Scheme Design
Considering the anticipated scale of real-world usage, special attention was given to the database scheme. The Bet table was strategically partitioned based on the addition date to manage potentially high volumes of data efficiently.

## Technology Selection
Given the emphasis on performance and the absence of complex data models, Micro ORM Dapper was chosen as the preferred technology for database interactions. Additionally, the Result Pattern was favored for validating business logic, prioritizing readability, flow control, and performance. Middleware was employed to manage unexpected server errors effectively.

## Handling Race Conditions
Managing race conditions proved to be a significant challenge. For instance, when placing a bet, blocking the existing user's balance introduced the risk of deadlocks. To mitigate this issue, a common practice of restarting transactional queries in case of deadlock was implemented.

## Security Measures
In consideration of sensitive user information such as passwords stored in a hashed state, measures were taken to ensure robust security. Hashing algorithms were employed to generate unique hashed passwords for each user, enhancing data protection.

## Testing Approach
While acknowledging the value of unit tests for complex logic, a focused approach was taken to write tests specifically targeting bet placement and token validation logic. This ensured critical functionalities were thoroughly validated.

## Conclusion
In conclusion, meticulous attention was paid to every aspect of the project, from structural design to security measures, to deliver a robust and efficient application tailored to meet the specified requirements.

