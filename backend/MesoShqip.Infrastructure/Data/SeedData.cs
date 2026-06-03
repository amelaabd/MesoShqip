using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MesoShqip.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Lessons.AnyAsync()) return;

        var lesson1 = new Lesson
        {
            TitleAlbanian = "Shtëpia dhe Familja",
            TitleEnglish = "Home and Family",
            Level = LanguageLevel.Fillestor,
            LessonType = LessonType.Vocabulary,
            OrderIndex = 1,
            IsPublished = true
        };

        var lesson2 = new Lesson
        {
            TitleAlbanian = "Ngjyrat dhe Format",
            TitleEnglish = "Colors and Shapes",
            Level = LanguageLevel.Fillestor,
            LessonType = LessonType.Vocabulary,
            OrderIndex = 2,
            IsPublished = true
        };

        var lesson3 = new Lesson
        {
            TitleAlbanian = "Natyra dhe Kafshët",
            TitleEnglish = "Nature and Animals",
            Level = LanguageLevel.Fillestor,
            LessonType = LessonType.Vocabulary,
            OrderIndex = 3,
            IsPublished = true
        };

        var lesson4 = new Lesson
        {
            TitleAlbanian = "Ushqimet dhe Pijet",
            TitleEnglish = "Food and Drinks",
            Level = LanguageLevel.Mesatar,
            LessonType = LessonType.Vocabulary,
            OrderIndex = 4,
            IsPublished = true
        };

        await context.Lessons.AddRangeAsync(lesson1, lesson2, lesson3, lesson4);
        await context.SaveChangesAsync();

        var vocab1 = new List<VocabularyItem>
        {
            new() { LessonId = lesson1.Id, WordAlbanian = "shtëpia",  WordEnglish = "the house",       Phonetic = "shTEH-pia",   ExampleSentence = "Shtëpia jonë është e madhe.",    DifficultyScore = 1 },
            new() { LessonId = lesson1.Id, WordAlbanian = "babai",    WordEnglish = "father",           Phonetic = "BA-bai",      ExampleSentence = "Babai im është i mirë.",         DifficultyScore = 1 },
            new() { LessonId = lesson1.Id, WordAlbanian = "nëna",     WordEnglish = "mother",           Phonetic = "NUH-na",      ExampleSentence = "Nëna gatuan bukur.",             DifficultyScore = 1 },
            new() { LessonId = lesson1.Id, WordAlbanian = "motra",    WordEnglish = "sister",           Phonetic = "MOT-ra",      ExampleSentence = "Motra ime është e vogël.",       DifficultyScore = 1 },
            new() { LessonId = lesson1.Id, WordAlbanian = "vëllai",   WordEnglish = "brother",          Phonetic = "VLA-i",       ExampleSentence = "Vëllai im luan futboll.",        DifficultyScore = 1 },
            new() { LessonId = lesson1.Id, WordAlbanian = "gjyshi",   WordEnglish = "grandfather",      Phonetic = "DYOO-shi",    ExampleSentence = "Gjyshi rrëfen tregime.",         DifficultyScore = 2 },
            new() { LessonId = lesson1.Id, WordAlbanian = "gjyshja",  WordEnglish = "grandmother",      Phonetic = "DYOOSH-ya",   ExampleSentence = "Gjyshja bën byrek.",             DifficultyScore = 2 },
            new() { LessonId = lesson1.Id, WordAlbanian = "dera",     WordEnglish = "the door",         Phonetic = "DEH-ra",      ExampleSentence = "Dera është e hapur.",            DifficultyScore = 1 },
        };

        var vocab2 = new List<VocabularyItem>
        {
            new() { LessonId = lesson2.Id, WordAlbanian = "i kuq",    WordEnglish = "red",              Phonetic = "ee KOOQ",     ExampleSentence = "Molla është e kuqe.",            DifficultyScore = 1 },
            new() { LessonId = lesson2.Id, WordAlbanian = "i blertë", WordEnglish = "green",            Phonetic = "ee BLER-tuh", ExampleSentence = "Bari është i blertë.",           DifficultyScore = 1 },
            new() { LessonId = lesson2.Id, WordAlbanian = "i kaltër", WordEnglish = "blue",             Phonetic = "ee KAL-tur",  ExampleSentence = "Qielli është i kaltër.",         DifficultyScore = 1 },
            new() { LessonId = lesson2.Id, WordAlbanian = "i bardhë", WordEnglish = "white",            Phonetic = "ee BAR-dhuh", ExampleSentence = "Bora është e bardhë.",           DifficultyScore = 1 },
            new() { LessonId = lesson2.Id, WordAlbanian = "i zi",     WordEnglish = "black",            Phonetic = "ee ZEE",      ExampleSentence = "Nata është e zezë.",             DifficultyScore = 1 },
            new() { LessonId = lesson2.Id, WordAlbanian = "i verdhë", WordEnglish = "yellow",           Phonetic = "ee VER-dhuh", ExampleSentence = "Dielli është i verdhë.",         DifficultyScore = 1 },
        };

        var vocab3 = new List<VocabularyItem>
        {
            new() { LessonId = lesson3.Id, WordAlbanian = "shqiponja", WordEnglish = "eagle",           Phonetic = "shchi-PON-ya", ExampleSentence = "Shqiponja fluturon lart.",      DifficultyScore = 2 },
            new() { LessonId = lesson3.Id, WordAlbanian = "mali",      WordEnglish = "the mountain",    Phonetic = "MA-lee",       ExampleSentence = "Mali është i lartë.",           DifficultyScore = 1 },
            new() { LessonId = lesson3.Id, WordAlbanian = "lumi",      WordEnglish = "the river",       Phonetic = "LOO-mee",      ExampleSentence = "Lumi rrjedh ngadalë.",          DifficultyScore = 1 },
            new() { LessonId = lesson3.Id, WordAlbanian = "pylli",     WordEnglish = "the forest",      Phonetic = "PYUL-lee",     ExampleSentence = "Pylli është i gjelbër.",        DifficultyScore = 2 },
            new() { LessonId = lesson3.Id, WordAlbanian = "qeni",      WordEnglish = "the dog",         Phonetic = "QYEH-nee",     ExampleSentence = "Qeni im është i zgjuar.",       DifficultyScore = 1 },
            new() { LessonId = lesson3.Id, WordAlbanian = "macja",     WordEnglish = "the cat",         Phonetic = "MA-tsya",      ExampleSentence = "Macja fle gjatë ditës.",        DifficultyScore = 1 },
        };

        var vocab4 = new List<VocabularyItem>
        {
            new() { LessonId = lesson4.Id, WordAlbanian = "buka",     WordEnglish = "the bread",        Phonetic = "BOO-ka",      ExampleSentence = "Buka është e nxehtë.",           DifficultyScore = 1 },
            new() { LessonId = lesson4.Id, WordAlbanian = "uji",      WordEnglish = "the water",        Phonetic = "OO-yee",      ExampleSentence = "Uji është i ftohtë.",            DifficultyScore = 1 },
            new() { LessonId = lesson4.Id, WordAlbanian = "qumështi", WordEnglish = "the milk",         Phonetic = "choo-MESH-tee",ExampleSentence = "Qumështi është i bardhë.",      DifficultyScore = 2 },
            new() { LessonId = lesson4.Id, WordAlbanian = "molla",    WordEnglish = "the apple",        Phonetic = "MOL-la",       ExampleSentence = "Molla është e kuqe dhe e ëmbël.",DifficultyScore = 1 },
            new() { LessonId = lesson4.Id, WordAlbanian = "byreku",   WordEnglish = "the börek",        Phonetic = "BYU-re-koo",   ExampleSentence = "Byreku i gjyshes është shumë i mirë.", DifficultyScore = 2 },
        };

        await context.VocabularyItems.AddRangeAsync(vocab1);
        await context.VocabularyItems.AddRangeAsync(vocab2);
        await context.VocabularyItems.AddRangeAsync(vocab3);
        await context.VocabularyItems.AddRangeAsync(vocab4);
        await context.SaveChangesAsync();
    }
}