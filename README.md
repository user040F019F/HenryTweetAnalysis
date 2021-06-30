## Configuration
To get the application to work properly, you will need to add a few settings. These settings are not included in this repository since they contain private information. Instead, you should manage the user secrets for the Henry.Tester project. See the below json for what your user settings configuration should look like:
```json
{
  "emojis": {
    "JsonFileLocation": "ABSOLUTE-PATH-TO-YOUR-SOLUTION-COPY-OF 'EmojiDefinitions.json'"
  },
  "client": {
    "ConsumerKey": "YOUR-TWITTER-CONSUMER-KEY",
    "ConsumerSecret": "YOUR-TWITTER-CONSUMER-SECRET"
  }
}
```