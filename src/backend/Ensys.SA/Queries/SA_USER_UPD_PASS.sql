UPDATE  SA_USER_H
SET     PWD_USER = @HashedPassword,
        YN_PWDCHANGE = N'N'
WHERE   ID_USER = @UserId