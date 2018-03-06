#!/bin/bash

if [ $# -ne 2 ]
then
	echo "Usage: ${0} yaml_file_name build_number"
	exit 1
fi

echo "update file ${1}, build number ${2}"
yaml_file=$1
build_number=$2
sed -i s/sia-playbook-ppe:latest/sia-playbook-ppe:$build_number/ $yaml_file 