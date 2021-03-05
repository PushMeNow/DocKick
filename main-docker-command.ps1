# docker help this parameter (--help) you can use for any cammnad 
docker --help

# create docker image locally by dockerfile
# where -t - parameter for your image name
docker build -t your_image_name .

# run created image
# it is simple example so for more options you can use compose file and commands 
docker run -it -p 5000:80 image_name

# run docker-compose file to launch container
docker-compose up --build -d