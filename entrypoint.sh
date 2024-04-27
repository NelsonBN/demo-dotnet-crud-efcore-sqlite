#!/bin/bash

if [ ! -f /data/app.db ]; then
    echo "Copying database file to volume"
    cp /app/data/app.db /data/app.db
fi

exec dotnet Demo.Api.dll
