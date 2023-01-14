#CI/CD Template

This template provides a basic setup for a CI/CD pipeline using an containerized nginx reverse proxy, a micro frontend, and a micro backend.
Requirements

    Docker
    Docker Compose

Usage

    Clone the repository.
    Add secrets to github
    Access the application on http://localhost:80.

CI/CD Pipeline

The pipeline is configured to automatically build and deploy the application whenever changes are pushed to the master branch.
Troubleshooting

If you encounter any issues, check the logs using docker-compose logs or portainer and make sure that all required ports are available.
Note

    Update the config files with the name of the container.

Tech Stack

    nginx
    Docker
    Docker Compose
    Microservices

Contribution

If you find any bugs or have suggestions for improvements, feel free to open an issue or make a pull request.
