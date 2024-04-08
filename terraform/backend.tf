terraform {
  backend "s3" {
    bucket  = "spectacle-stack-tfstate-bucket" 
    key     = "csharplevelup/terraform.tfstate" # location of the terraform state file;to store the state of the deployment infrastructure
    region  = "eu-west-1"
  }
}