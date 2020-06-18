In this document we will describe the intricacies of this authentication project.
To use this authentication mechanism you will need 3 sql databases. 
The sql script to make a setup of these 3 sql databases can be found within this project.
After you have these databases initialized you can make make use of the authentication service.

GDPR:
According to law, we do NOT have any obligations towards the GDPR.
However, we have multiple security measures in place to make
secure data storage easy when implementing this system.
These security measures are:
    
    - Argon2
    This authentication project uses the secure hashing algorithm argon2 to protect user passwords.
    This hashing algorithm won the Password Hashing competition in July 2015. It is a very secure
    algorithm with high defence against multiple types of attack, which is why we have chosen this
    algorithm in stead of others.
    
    - Encryption of personal data
    In addition to the secure storage of user passwords, we also have a encryption algorithm in place
    to protect personal data. This hashing algorithm is called AES and is a fairly standard algorithm.

    - Token authentication
    The authentication used in this project is largely based on JWT token authentication. When a user logs
    in with the right credentials, a token is generated with the credentials of the user in question.
    Within this token an expiration time is set. This means the token is only valid for a certain amount of time.
    We have set this value to two minutes with one minute clock skew. After two minutes the user has to refresh 
    his/her token to keep access to the service provided. Because it is very user unfriendly to make users 
    refresh their credentials every two minutes, this application automatically lets users refresh their token.
    With this implementation they gain a fresh token with a new expiration date. This way the old token is
    not valid anymore and if it gets stolen becomes useless, but because of the refresh token the user can keep
    using the provided service.
    
    - Data seperation
    Because we are using three different databases to store user data and other data used to authenticate the user,
    it is harder for malificent users to try to gain access to personal information or gain access to somebody's
    account. Somebody needs to gain access to all three databases to gain access to the personal data.
    
Best Practices:
If you want to correctly implement this service, we recommend you take some other measure to provide safety to users
and their personal data.
These measures are:
    
    - Custom firewall rules
    You should make custom rules to only let certain IP addresses make use of this authentication service and set up a blacklist.
    
    - Multiple networks
    Seperate the actual databases from the service to make it more safe and add a firewall to add certain rules to gain access
    to the databases.
    
    - Proper logging
    Log all instances of weird access or new access to databases and log wrong calls to the database. You should also regularly
    check these logs for inconsistencies and warnings.
    
    - Physical security
    You should always have a form of physical security in place, I.E. somebody should not be able to just walk into your business.
    
    - Proper data access
    Only certain people should be able to gain access to data, I.E. administrators. They should not have access to all three of 
    the databases, because they should not have direct access to personal data. People with access should also be required to 
    change their password once in a while. When users change, passwords should be changed immediately.
    
    - Awareness spreading
    Spread awareness within your business about the security concerns. Also tell users they should not click phishing e-mails and
    such.
    
    - IDS/IPS implementation
    Implement an IDS/IPS to detect and prevent intruders from gaining access.
    
If you have any other questions, feel free to ask.
    
Kind regards,
Secure Authentication