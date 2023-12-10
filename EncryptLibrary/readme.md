## About library
This is library for encrypting and decrypting files.

## Structure
### Interfaces
- IFileEncrypter - base for all Encrypter implementations, should implement Encrypt, Decrypt and async variation methods.

- IEncryptingAlgorithm - base for different encrypting algorithms, should implement EncryptByte and DecryptByte.

### Classes
- FileEncrypter - class for encrypting and decrypting your file.
- FileEncryptionResult - class for providing info about encrypting and decrypting such as file size, duration of encrypting and etc.

- MathExtensions - class helper for IEncryptingAlgorithm implementation.