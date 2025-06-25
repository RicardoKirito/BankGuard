# BankGuard
**BankGuard** is a C#/APS.NET Core Application which allow you to manage bank transactions and accounts. It integrate authentication (implementing Identity) and Entity Framework as an **ORM**  to manage data persistance. 

## Instalation
### 
###**Recomended Setup**
Open Visual Studio 2022 and choose clone repository and use the this link:  https://github.com/RicardoKirito/BankGuard.git

 One you clone the repository follow this steps: 
 
#### **Data Base Setup**
Go to your *SQL Server Management Studio app* and create a Data base named **"Bank"**.

#### **Update the appsettings.json**
Open the appsettings.json and change the Server name located in the **"ConnectionStrings"** string to your Server name. 
Now the data base is created and the right information is in the **"ConnectionStrings"** update it with the migrations information. To do so follow this steps: 
1. Open the **NuGets Package Manager Terminal** make sure you select the project BankGuard.Infrastructure.Persistence and run the next comand: `update-database -context  ApplicationContext`.
1. Using the same **NuGets Package Manager Terminal** now change the project to BankGuard.Infrastructure.Identity and run the next comand: `update-database -context  IdentityContext`.

##Roles and access
- **Admin**
This is the role the application manager should use. This role has the following features: 
	- **Create, update, disactivate or active** any user. 
	**Create, update and update** any users account.
- **Basic User**
This user can run transactions, add beneficiaries, make payments and any bank activity related to his accounts.
- **Access**
The default users integrated with the app have the same password: `Coron@123`
## Licence
MIT
