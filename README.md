# Introduction 

# CryptoZ API

**CryptoZ** is a **cryptocurrency** convertion web page and this project is it's API.


## Getting Started

1.	**Clone** the repository
2.  **Run** Program.cs


## API Reference

### Endpoints

#### Countries
> **GET** /countries

Returns all possible countries.


> **GET** /countries/{code}

Returns a specific country by it's code.


#### Currencies
> **GET** /currencies

Returns all possible currencies.


> **GET** /currencies/{code}

Returns a specific currencies by it's code.


#### History
> **GET** /history
>
> **Parameters:**
>
> _userId_ : int
>
> _limit_ : int

Returns all possible currencies.


> **POST** /history

Creates a history and saves it to the DB.


#### Users
> **GET** /users/{id}

_In development_<br>
Returns a specific user by it's id.


> **POST** /users

Creates a user and saves it to the DB.


> **PUT** /users/{id}

_In development_<br>
Modifies a user and saves it to the DB.

### Schemas

####CurrencyForViewDto

        {
            code*	        string
                            maxLength: 10
            name*	        string
                            maxLength: 25
            price*	        number($double)
            priceDate*	string($date-time)
            logoUrl	        string
                            nullable: true
        }

####HistoryForViewDto
        {
            originCode*	        string
            destinationCode*	string
            value*	                number($double)
            result*	                number($double)
            date*	                string($date-time)
        }

####HistoryForCreationDto
        {
            originCode*	    string
            userEmail           string
                                nullable: true
            destinationCode*    string
            value*	            number($double)
        }

####UserForViewDto
        {
            name*	            string
                                maxLength: 64
            email*	            string
                                maxLength: 320
        }

####UserForCreationDto
        {
            name*	            string
                                maxLength: 64
            email*	            string
                                maxLength: 320
            password*	    string
                                minLength: 8
        }

####UserForUpdateDto
        {
            name	            string
                                maxLength: 64
                                nullable: true
            email*	            string
                                maxLength: 320
            password*	    string
                                minLength: 8
            newPassword	    string
                                minLength: 8
                                nullable: true
        }

