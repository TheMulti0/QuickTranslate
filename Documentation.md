# Getting started
**EasyTranslate** has only one translator one, it's called ``GoogleTranslator`` which uses the [classic Google Translate webpage](https://translate.google.com/).
So the first thing you have got to do is to create that translator. 
 
```cs
ITranslator translator = new GoogleTranslator();
```
Now if you want to asynchronously translate a word, you've got to create a new ``TranslationSequence`` that **its** 'sequence' property is the sequence that **you** desire to translate, and specify the language you want to translate to:
```cs
Task<TranslationSequence> result = await translator.Translate(new TranslationSequence("Bonjour"), TranslateLanguages.English);
```

Let's say you want to detect a word / sentences' language, you've got to create a new TranslationSequence, specify your word.
After that, the detect method should return you a ``TranslationSequence`` which includes your word, and it's (detected) language!
```cs
Task<TranslationSequence> result  = await translator.Detect(new TranslationSequence("Hello there"));
``` 

# More
I recommend you all read the [full documentation](https://github.com/TheMulti0/EasyTranslate/wiki) in the repository wiki.
So that's it! Hope you have fun using my package, and please contact me if you find any issue / suggestion!