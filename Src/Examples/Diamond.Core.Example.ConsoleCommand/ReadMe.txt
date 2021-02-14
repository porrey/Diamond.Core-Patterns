Here are some commands to try:

create --number INV12345 --description "Shipment invoice" --total 125.00 --paid true
update --number INV12345 --description "Shipment invoice updated" --total 125.60 --paid true
list
get --number INV12345
delete --number INV12345