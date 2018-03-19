#!/bin/bash

if [ $# -ne 3 ]
then
	echo "Usage: ${0} yaml_file_name image_repo build_number"
	exit 1
fi

echo "update file ${1}, image repo ${2}, build number ${3}"
yaml_file=$1
image_repo=$2
build_number=$3

grep -q $image_repo $yaml_file
if [ $? -eq 0 ]
then
  sed -i s/$image_repo:latest/$image_repo:$build_number/ $yaml_file
else
  echo "$image_repo is not in $yaml_file."
  exit 2
fi
