# PaymentApi

PaymentApi is a sample API implementation that mimics an API Gateway for merchants to integrate with acquiring banks.

## Considerations

* The objective of this API Gateway is to provide an unified entry point for interacting with acquiring banks;
* At this moment, the API works as a pass-through, having no sort of data persistence by design. In order to implement a proper storage solution, it should be clear what information is relevant to the platform, but most of all, what legally can be stored on it;
* A *CardHolder* information was added to the payment details endpoint in order to fulfill the requirement of returning card details. Assuming that sensible information such as card expiration and CVV should not either be returned or stored and given that the card number itself should be masked, there was no other information left to be considered as *card details*;
* The API segregates the internal unique identifier (the one returned by the acquiring bank) from the one handled by its endpoints. Usually this is implemented by having some backing model persisting the 1-to-1 mapping between these two identifiers;
  * Given that no backing model was implemented, in order to fulfill this requirement the service uses symmetric AES encryption to map the acquiring bank's 128 bit GUID into a 128 bit GUID that is exposed outside;
* Considering that some acquiring banks could fulfill this sort of request asynchronously, the `POST /payment` endpoint returns a `Created 201` with a location to the payment resource instead of returning a `OK 200` with the status message;
* The acquiring bank mocking solution was hosted together with the project instead of on a separate solution (ex.: Docker container with *MockServer*) for personal preferences, as well as making it easier to configure the mocks for component testing;

## *AcquiringBankMock* behaviour

The acquiring bank mock follows these rules, defined on `FakeAcquiringBankMockSettings.cs`:
* The `GET` endpoint is dynamically configured by the `POST` invokes, simulating a creation behaviour;
* The `POST` endpoint has few predefined patterns that simulates validations:
  * Card Expiration (Month/Year) past current Month/Year provides an `EXPIRED_CARD` status;
  * Card Number with less than 16 digits provides an `INVALID_CARDNUMBER` status;
  * Amount less than or equal to zero provides an `INVALID_AMOUNT` status;
  * CVV with less than 3 digits provides an `INVALID_CVV` status;
  * If no other pattern matches, provides a `SUCCESS` status;

## Additional Information
The solution includes:
* [*Swagger*](https://swagger.io/) for Api browsing;
* [*NLog*](https://nlog-project.org/) for logging;
* An Api Client for easily consuming the PaymentApi;

## Running the application in a Linux Docker container
Having docker installed on your machine, run a command terminal, go to your project root path and run these commands:

```
>> docker-compose build
>> docker-compose up
```

The application runs by default on port `44339`.

## Author

- Daniela Mesquita Zamberlan 
- Mail: danizamberlan@gmail.com
- Linkedin: [www.linkedin.com/in/daniela-zamberlan](www.linkedin.com/in/daniela-zamberlan)

# API Reference

## Process a payment

```http
POST /payment
```

**Request**
| Parameter | Type | Description |
| :--- | :--- | :--- |
| `CardNumber` | `ulong` | **Required**. Card number whose funds will be captured. |
| `CardExpirationMonth` | `short` | **Required**. Card expiration month (1-12). |
| `CardExpirationYear` | `short` | **Required**. Card expiration year (4 digits). |
| `CVV` | `short` | **Required**. CVV number (3 digits). |
| `Amount` | `decimal` | **Required**. Amount to be captured. |
| `Currency` | `string` | **Required**. Currency code (3 letters). |

**Responses**
| Code| Description |
| :--- | :--- | 
| `Created 201`| Payment created. `Location` header contains the URL for fetching the details. 
| `Bad Request 400` | Api Gateway validations or acquiring bank errors. |

## Retrieve payment details

```http
GET /payment/{PaymentId}
```

**Request**
| Parameter | Type | Description |
| :--- | :--- | :--- |
| `PaymentId` | `Guid` | **Required**. Payment unique identifier provided by the PaymentApi. |

**Responses**
| Code| Description |
| :--- | :--- | 
| `OK 200`| Payment details. See *PaymentDetails* for response description. |
| `Not Found 404` | No payment details found for a given PaymentId. |

***PaymentDetails* response**
| Field| Type | Description |
| :--- | :--- | :--- |
| `PaymentId` | `Guid` | Payment identifier. |
| `CardNumber ` | `string` | Masked card number. By default, exposes the last 4 digits of the card number. |
| `CardHolder` | `string` | Card holder name as informed by the acquiring bank. |
| `Amount` | `decimal` | Amount to be captured. |
| `Currency` | `string` | Currency code (3 letters). |
| `PaymentStatus` | `string` | Status of the payment request. |
