# configured aws provider with proper credentials
provider "aws" {
  region  = "eu-west-1"
}

# create the rds instance
module "db" {
  source                    = "terraform-aws-modules/rds/aws"
  identifier                = "spectacles-stack-rds-instance"
  engine                    = "postgres"
  engine_version            = "16"
  family                    = "postgres16"
  major_engine_version      = "16"            
  instance_class            = "db.t3.micro"
  allocated_storage         = 20
  max_allocated_storage     = 30
  storage_encrypted         = false
  username                  = "bbdGradWandile"
  db_name                   = "SpectablesStackDB"
  port                      = 5432
  publicly_accessible       = true
  db_subnet_group_name      = "spectacles-stack-db-subnet-group"
  vpc_security_group_ids    = ["sg-012b3f4f0396e4c9a"]
  multi_az                  = false

  backup_retention_period   = 1
  skip_final_snapshot       = true
  deletion_protection       = false
  create_db_option_group    = false
}

module "ec2-instance" {
  source                       = "terraform-aws-modules/ec2-instance/aws"
  name                         = "spectacles-stack-api-server"
  key_name                     = "spectacles-stack-api-ssh-key"
  instance_type                = "t2.micro"
  ami_ssm_parameter            = "/aws/service/canonical/ubuntu/server/22.04/stable/current/amd64/hvm/ebs-gp2/ami-id"
  vpc_security_group_ids       = ["sg-012b3f4f0396e4c9a"]
  subnet_id                    = "subnet-032070b4837c85e0e"
  associate_public_ip_address  = true
}

output "ec2_public_ip" {
  value = module.ec2-instance.public_ip
}

module "ecr" {
  source = "terraform-aws-modules/ecr/aws"

  repository_name = "spectacles-stack-api-repo"
  create_repository = false

  repository_lifecycle_policy = jsonencode({
    rules = [
      {
        rulePriority = 1,
        description  = "Keep last 7 images",
        selection = {
          tagStatus     = "tagged",
          tagPrefixList = ["v"],
          countType     = "imageCountMoreThan",
          countNumber   = 7
        },
        action = {
          type = "expire"
        }
      }
    ]
  })

  tags = {
    Terraform   = "true"
    Environment = "dev"
  }
}