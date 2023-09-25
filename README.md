# Bachelor's Project - Master Application with Proxy Communication

## Introduction

This repository represents the culmination of a Bachelor's project. The project centers around the development of a Master Application that communicates with a Slave Simulator through a proxy-based architecture. The primary objective of this endeavor is to establish secure communication between the Master Application and the Slave Simulator while maintaining data integrity and confidentiality. This is accomplished through the implementation of various security measures, including signing and encryption.

## Project Overview

In this project, a Master Application has been implemented to establish communication with a Slave Simulator. This communication is facilitated through proxy servers situated at both ends of the connection. The communication between these proxies is established over TCP, while direct communication between the Master Application and the Slave Simulator is conducted using the Modbus/TCP protocol.

### Key Features

1. **Proxy Communication**: Proxy servers have been integrated at both the Master Application and Slave Simulator ends to ensure secure data transmission.

2. **Modbus/TCP Protocol**: The project employs the Modbus/TCP protocol for direct communication with the Slave Simulator, a widely used industrial communication protocol.

3. **Security Measures**: Various security measures, such as signing and encryption, have been incorporated into the communication process to safeguard the data.

## Repository Structure

This repository houses the source code for the Master Application and the proxy servers utilized in the project. The communication mechanisms within the project have been designed to prioritize security and reliability.

## Related Repository

As part of this Bachelor's project, another project had been created to complement the work presented here. This related project focuses on intercepting and monitoring the messages exchanged between the proxy servers using WinDivert. This additional application is used to demonstrate how the implemented security measures protect the communication between proxies.

To explore the related repository, please visit https://github.com/mpotic/my-sharp-divert.

## Getting Started

Users are encouraged to explore the code and documentation available in this repository and its related counterpart. If there are any inquiries or feedback, please do not hesitate to contact the author.

**Author:** Miloš Potić
**Date:** 15.09.2023.