##DAY 1
#Create Auth
pac auth create -env {url_env}

#Export Unamnaged Solution
pac solution export --path solutions/con_coresolution.zip --managed false --name con_coresolution  --include general

#Export Managed Solution
pac solution export --path solutions/con_coresolution_managed.zip --managed true --name con_coresolution  --include general

#Unpack
pac solution unpack -z solutions/con_coresolution.zip -f solutions/con_coresolution -p both

##DAY 2
#Create settings
pac solution create-settings --solution-zip .\solutions\con_coresolution.zip --settings-file .\deployment\con_coresolution.json     