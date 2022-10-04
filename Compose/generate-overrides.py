from glob import glob
import os
import re

compose_root = os.path.dirname(os.path.realpath(__file__))
secrets_root = [y for x in os.walk(compose_root) for y in glob(os.path.join(x[0], "Secrets"))][0]
compose_files = [y for x in os.walk(compose_root) for y in glob(os.path.join(x[0], "docker-compose.yml"))]
secrets_files = [y for x in os.walk(secrets_root) for y in glob(os.path.join(x[0], "*.secret"))]
secrets = dict()
for secret_file in secrets_files:
    key = os.path.basename(os.path.splitext(secret_file)[0])
    with open(secret_file, "r") as reader:
        secrets[key] = reader.readline().strip()
def replace_secret(match):
    match_value = match.string[match.start():match.end()]
    key = re.sub(r"SECRET::", "", match_value)
    if key in secrets:
        return secrets[key]
    return match_value
for compose_file in compose_files:
    with open(compose_file, "r") as reader:
        content = reader.read()
    content = re.sub(r"SECRET::([a-zA-Z0-9_])+", replace_secret, content)
    with open(re.sub(r"docker-compose.yml$", "docker-compose.generated.yml", compose_file), "w") as writer:
        writer.write(content)
