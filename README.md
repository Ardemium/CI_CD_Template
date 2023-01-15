CI/CD Template
This is a CI/CD template for automating the deployment of containerized microservices on a server.

Prerequisites
SSH access to the server
GitHub repository for storing secrets
Docker and Docker Compose installed on the server

Setup
Clone this repository to the server location after an SSH connection.
Add HOST, USERNAME, and KEY as secrets in your GitHub repository.
Generate an SSH key using ssh-keygen -t rsa and add it to the server's authorized keys.
Update the docker-compose.yml and nginx.conf files for adding new microservices.
Configure firewall and limit user's sudo privileges to necessary commands such as docker-compose up.
Use SSH certificates and do not store the private key on the server.

Deployment
To deploy updates, push changes to the GitHub repository and they will be automatically pulled and deployed on the server.

Communication
Communication between containers is controlled by an nginx reverse proxy container.

Tips
Test changes locally before deploying to the server
Keep the server's package manager updated
Regularly check logs for errors and issues
Feel free to use, modify, and distribute this template.
It's a comprehensive solution but it may not cover all scenarios and may contain errors.
It's recommended to test it locally and customize it to fit your needs.
