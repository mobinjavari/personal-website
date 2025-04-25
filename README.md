<div align="center">
    <h1>🌟 Personal Website <sub>with</sub> ASP.NET Core</h1>
    <p>
        Modern personal website with clean design and responsive interface
    </p>
    <br><br>
</div>

## Installation

1. Clone the repository

```bash
git clone https://github.com/mobinjavari/personal-website-cs.git
cd personal-website-cs
```

2. Run the project

```bash
dotnet run
```

The application will be available at [http://localhost:5214](http://localhost:5214)

## Docker

1. Build the project

```bash
docker build -t personal-website .
```

2. Run the project

```bash
docker run -d --name my-web-app -p 8080:8080 personal-website
```

The application will be available at [http://localhost:8080](http://localhost:8080)

## Technologies

- ASP.NET Core (C#)
- Tailwind CSS
- Docker
