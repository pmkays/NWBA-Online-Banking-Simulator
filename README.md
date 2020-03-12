# NWBA Internet Banking Simulator
## Description

This web application aims to mimic the workings of an internet banking system of the hypothetical NWBA (National Wealth Bank of Australasia) by utilising ASP.<span></span>NET Core 3.1 using C#, and Entity Framework Core with a local database backend. 

Features of the web application include being able to: 

- Check balances of accounts

- Modify the personal profile of a customer

- Simulate transactions such as deposits, withdrawals and transfers

- Schedule payments (BPAY)

- Log In/Log out

- View statements


Features of the admin application and web API include being able to:

- View transaction history of any customer

- Generate graphs and statistics of account usage

- Modify/Update/Delete Customer Details

- Locking and unlocking of Customer Accounts

- Locking and unlocking of Schedule payments (BPAY)

- Utilisation of an API to do the above functions

## Instructions
Inside, there are two solutions (three projects); please run the NWBA_Web_Application.<span></span>sln (in NWBA_Web_Application directory) before the NWBA_Web_Admin.<span></span>sln (in NWBA_Web_Admin directory). In the NWBA_Web_Admin.<span></span>sln, please run the WebApi project first and leave it in the background before running the NWBA_Web_Admin project. 

For NWBA_Web_Application login:
Username: 12345678
Password: abc123

For NWBA_Web_Admin login:
Username: admin
Password: admin

## Authors
Paula Kurniawan & Ian Nguyen
