# Getting started
**EasyTranslate** has only one translator one, it's called ``GoogleTranslateClassicTranslator`` which uses the [classic Google Translate webpage](https://translate.google.com/).
So the first thing you gotta do is to create that translator. 
 
```cs
ITranslator translator = new GoogleTranslateClassicTranslator();
```
Now if you want to translate a word, you've got to create a new ``TranslateWord`` that **its** word property is **your** word / sentence, and specify the language you want to translate it to:
```cs
TranslateWord result = translator.Translate(new TranslateWord("Bonjour"), TranslateLanguages.English)
```

Let's say you want to detect a word / sentences' language, you've got to create a new TranslateWord, specify your word.
After that, the detect method should return you a ``TranslateWord`` which includes your word, and it's (detected) language!
```cs
TranslateWord result  = translator.Detect(new TranslateWord("Hello there"));
``` 

# More
I recommend you all read the [full documentation](https://github.com/TheMulti0/EasyTranslate/wiki) in the repository wiki.
So that's it! Hope you have fun using my package, and please contact me if you find any issue / suggestion!