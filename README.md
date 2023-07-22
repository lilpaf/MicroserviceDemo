# MicroserviceDemo
This is demo project that is aimed to show microservice architecture

### Requirments

#### Docker and Kubernetes
You need to install docker and kubernetes. You will need to run the docker files and apply all the yaml files.
Also don't forget to add the secrets.

#### MSSQL
You will need SQL Server Manegement Studio and connect to localhost, 1432 with user 'sa' and the appropriate password. That way you can accses the DB's  

#### Domain
I have used acme.com as host in the NGINX Ingress Controller, acme.com is bind to localhost port 127.0.0.1. You have to bind acme.com with 127.0.0.1 in /etc/hosts

#### RabbitMQ
You can access the RabbitMQ menegment on this url: 'http://localhost:15672/'. The password and username are the default ones 'guest'. 

#### HTTPS
If you want the NGINX Ingress Controller to handle HTTPS requests with valid (only on you mashine) SSL certificate follow this steps:

1. Instal openssl and run the following commands:
```
openssl genrsa -des3 -out myCA.key 2048

openssl req -x509 -new -nodes -key myCA.key -sha256 -days 365 -out myCA.pem

openssl genrsa -out acme.com.key 2048

openssl req -new -key acme.com.key -out acme.com.csr

openssl x509 -req -in acme.com.csr -CA myCA.pem -CAkey myCA.key -CAcreateserial -out acme.com.crt -days 825 -sha256 -extfile acme.com.ext
```

2. After that create a kubernetes secret
```
kubectl create secret tls nginx-tls-secret --cert=acme.com.crt --key=acme.com.key
```

3. Add the certificate on your local mashine
```
Click on the Start menu >> Run. Type in mmc and press OK

Click on File and choose the Add/Remove Snap-in option.

Select Certificates from the Available snap-ins list and click the Add button.

Choose Computer account to manage the certificate and click Next.

Select Local Computer and press the Finish button.

Certificates snap-in was selected. Click OK to add it to the console.

# Import intermediate/root certificates. 

To import an intermediate certificate, right-click on Intermediate Certification Authority >> All Tasks >> Import. Here I imported myCA.pem

# Import Trusted Root Certification Authorities.

To import an intermediate certificate, right-click on Trusted Root Certification Authorities >> All Tasks >> Import. Here I imported myCA.pem and acme.com
```

4. Finnaly change the ingress-srv.yml to include under spec this code
```
tls: 
    - hosts:
        - acme.com 
      secretName: nginx-tls-secret
```
