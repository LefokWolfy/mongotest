#!/bin/sh

echo "⏳ Waiting for MongoDB nodes to be available..."

# Wait for all nodes to respond
until mongosh --host mongo1 --eval "db.adminCommand('ping')" && \
      mongosh --host mongo2 --eval "db.adminCommand('ping')" && \
      mongosh --host mongo3 --eval "db.adminCommand('ping')"
do
  echo "🔁 Waiting for Mongo nodes..."
  sleep 2
done

echo "✅ All MongoDB nodes reachable, initializing replica set..."

mongosh --host mongo1 --eval 'rs.initiate({
  _id: "rs0",
  members: [
    { _id: 0, host: "mongo1:27017" },
    { _id: 1, host: "mongo2:27017" },
    { _id: 2, host: "mongo3:27017" }
  ]
})'
