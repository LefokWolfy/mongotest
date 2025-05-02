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

echo "🔁 Waiting for primary election"
sleep 10

PRIMARY=""
for HOST in mongo1 mongo2 mongo3; do
  IS_PRIMARY=$(mongosh --quiet --host $HOST --eval "db.isMaster().ismaster")
  if [ "$IS_PRIMARY" = "true" ]; then
    PRIMARY=$HOST
    echo "✅ Primary elected: $PRIMARY"
    break
  fi
done

if [ -z "$PRIMARY" ]; then
  echo "❌ Could not determine primary."
  exit 1
fi

echo "🛡️ Creating admin user on $PRIMARY..."

mongosh --host $PRIMARY --eval '
  db.getSiblingDB("admin").createUser({
    user: "root",
    pwd: "admin",
    roles: [ { role: "root", db: "admin" } ]
  })
'

echo "✅ Replica set initialized and admin user created."