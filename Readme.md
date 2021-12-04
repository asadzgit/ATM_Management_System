ATM Management System I had to create an ATM Management System using N-tier architecture in dotNet. The three layers in the architecture are named as View Layer, Logic Layer and Data Access Layer.

## R E Q U I R E M E N T S 
* There are two types of users, Customers and Administrators. Both are presented with their own menus after login. 
* Customers can use the system to withdraw cash, transfer cash from one account to another, deposit cash and get their current balance.

* Administrators can create, delete, view and update accounts of different users. They must also be able to view certain reports about users and accounts. All data should be stored in a File system.

* When your program starts, it should be displaying a login screen. User will be asked to enter a login and 5 digit pin code. The system verifies the login and pin and displays an error if it is incorrect. If the user types the pin code incorrectly three times consecutively then the system should disable that login until further notice (i.e. the Administrator changes the status of the user).

* The login information must be encrypted when it is stored to disk and decrypted when it is needed. We are going to use a very simple encryption technique which is as follows. For alphabets we swap A with Z, B with Y and so on. A B C D E F G H I J K L M N O P Q R S T U V W X Y Z Z Y X W V U T S R Q P O N M L K J I H G F E D C B A For Number we have 0123456789 9876543210
