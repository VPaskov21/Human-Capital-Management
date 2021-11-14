<h2 align="center">Human Capital Management App</h2>

The goal is to create an application which should store and manage employees information in a small/medium company.

## Description

The application consists of two divisions - one designed especially for the employees and another for the human resource specialists. 

The employees have the ability to see:
* their current employment status and salary; 
* information stored about them in the system - personal information, address;
* all employees in the company and also employees in their department alone;
* salary history entries with salary breakdown, deductions and amounts paid;
* calendar with all leaves for him and his colleagues;
* previous leave requests;
* request new paid/unpaid/sick/other reason leave;
* edit his profile;

The human resources specialists have the ability to (in addition to the abilities of the employee):
* see statistics about the company;
* add/edit/detele employees;
* see former employees;
* approve/reject leave requests;
* approve monthly salaries and assign bonuses;

Admin is special user in the application. The admin can:
* create departments;
* create roles;

### Built with:

- .NET Core 3.1
- Microsoft SQL Server
- Bootstrap
- jQuery
- Docker

## Getting Started

### Dependencies
* Docker Desktop

### Installing
1. Clone the repo
````
git clone https://github.com/VPaskov21/Human-Capital-Management.git
````

2. Change the DefaultConnectionString in appsettings.json

3. Build the application with Docker
````
docker build -t <docker-image-name> .
````
ex.
````
docker build -t hcmapp .
````

4. Create container for the application
````
docker create --name <docker-container-name> -p <port> <docker-image-name>
````
ex.
````
docker create --name hcm-container -p 8080:80 hcmapp
````

5. Run the container
````
docker start <docker-container-name>
`````
ex.
````
docker start hcm-container
`````

6. Launch web browser and visit localhost:8080

## Usage

Test accounts in the application:

HR Account
````
Username: 'KPopov'
Password: '123pass'
`````

Employee Accounts
`````
Usernames: { 'PIPetrov', 'IPStefanov', 'CDToncheva', 'ATAntonova', 'TFSpasov' }
Password: 'random'
`````

Admin account
`````
Username: 'admin'
Password: 'pass'
`````

## Acknowledgments

* [Nager.Date](https://date.nager.at/)
* [FullCalendar](https://fullcalendar.io/)
* [CyrillicConvert](https://www.nuget.org/packages/Cyrillic.Convert/)
