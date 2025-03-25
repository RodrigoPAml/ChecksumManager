# Checksum Manager

Terminal app to generate and verify checksums (MD5, SHA1, SHA256 and SHA512) of files in folder

## Checksum Generator

Generate a .txt with all files and checksums in the root folder with desired method

Outputs a file named checksum<CHECKSUM_METHOD>.txt

```
ChecksumGenerator <ROOT_FOLDER> <OUTPUT_FOLDER> <CHECKSUM_METHOD>
```
![image](https://github.com/user-attachments/assets/9daed7ba-9a23-4f2f-938a-bfca6e513e01)

## Checksum Checker

Verify if all the files in the root folder match the checksum provided by the checksum file

Output errors in the verifition, if any

```
ChecksumChecker <ROOT_FOLDER> <CHECKSUM_FILE> <METHOD>
```

![image](https://github.com/user-attachments/assets/f642e404-31d7-40d7-963c-c9a9c5d6a63d)

### Fail

In case of fail of any nature it will point

![image](https://github.com/user-attachments/assets/ff04207b-9442-4fb5-b7d0-245a06f0046e)
