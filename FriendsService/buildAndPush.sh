#bin/sh
docker build -t cnazk/naabi-friends-service -f Dockerfile ..
docker push cnazk/naabi-friends-service
kubectl rollout restart deployment naabi-friends-service