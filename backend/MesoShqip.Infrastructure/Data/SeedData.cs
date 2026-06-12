using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MesoShqip.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!await context.Users.AnyAsync(u => u.Role == "Admin"))
        {
            var admin = new User
            {
                Username = "admin",
                Email = "admin@mesoshqip.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = "Admin",
                NativeLanguage = "en",
                Level = LanguageLevel.Avancuar,
                OnboardingCompleted = true
            };
            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }

        if (await context.Lessons.AnyAsync()) return;

        var lesson1 = new Lesson { TitleAlbanian = "Shtëpia dhe Familja", TitleEnglish = "Home and Family", Level = LanguageLevel.Fillestor, LessonType = LessonType.Vocabulary, OrderIndex = 1, IsPublished = true };
        var lesson2 = new Lesson { TitleAlbanian = "Ngjyrat dhe Format", TitleEnglish = "Colors and Shapes", Level = LanguageLevel.Fillestor, LessonType = LessonType.Vocabulary, OrderIndex = 2, IsPublished = true };
        var lesson3 = new Lesson { TitleAlbanian = "Natyra dhe Kafshët", TitleEnglish = "Nature and Animals", Level = LanguageLevel.Fillestor, LessonType = LessonType.Vocabulary, OrderIndex = 3, IsPublished = true };
        var lesson4 = new Lesson { TitleAlbanian = "Ushqimet dhe Pijet", TitleEnglish = "Food and Drinks", Level = LanguageLevel.Mesatar, LessonType = LessonType.Vocabulary, OrderIndex = 4, IsPublished = true };

        await context.Lessons.AddRangeAsync(lesson1, lesson2, lesson3, lesson4);
        await context.SaveChangesAsync();

        var vocab1 = new List<VocabularyItem>
        {
            new() { LessonId=lesson1.Id, WordAlbanian="shtëpia",  WordEnglish="the house",    WordGerman="das Haus",       WordItalian="la casa",       WordFrench="la maison",      WordSwedish="huset",          WordTurkish="ev",              Phonetic="shTEH-pia",  ExampleSentence="Shtëpia jonë është e madhe.",    DifficultyScore=1 },
            new() { LessonId=lesson1.Id, WordAlbanian="babai",    WordEnglish="father",       WordGerman="Vater",          WordItalian="padre",         WordFrench="père",           WordSwedish="pappa",          WordTurkish="baba",            Phonetic="BA-bai",    ExampleSentence="Babai im është i mirë.",         DifficultyScore=1 },
            new() { LessonId=lesson1.Id, WordAlbanian="nëna",     WordEnglish="mother",       WordGerman="Mutter",         WordItalian="madre",         WordFrench="mère",           WordSwedish="mamma",          WordTurkish="anne",            Phonetic="NUH-na",    ExampleSentence="Nëna gatuan bukur.",             DifficultyScore=1 },
            new() { LessonId=lesson1.Id, WordAlbanian="motra",    WordEnglish="sister",       WordGerman="Schwester",      WordItalian="sorella",       WordFrench="sœur",           WordSwedish="syster",         WordTurkish="k?z karde?",      Phonetic="MOT-ra",    ExampleSentence="Motra ime është e vogël.",       DifficultyScore=1 },
            new() { LessonId=lesson1.Id, WordAlbanian="vëllai",   WordEnglish="brother",      WordGerman="Bruder",         WordItalian="fratello",      WordFrench="frère",          WordSwedish="bror",           WordTurkish="erkek karde?",    Phonetic="VLA-i",     ExampleSentence="Vëllai im luan futboll.",        DifficultyScore=1 },
            new() { LessonId=lesson1.Id, WordAlbanian="gjyshi",   WordEnglish="grandfather",  WordGerman="Großvater",      WordItalian="nonno",         WordFrench="grand-père",     WordSwedish="morfar",         WordTurkish="büyükbaba",       Phonetic="DYOO-shi",  ExampleSentence="Gjyshi rrëfen tregime.",         DifficultyScore=2 },
            new() { LessonId=lesson1.Id, WordAlbanian="gjyshja",  WordEnglish="grandmother",  WordGerman="Großmutter",     WordItalian="nonna",         WordFrench="grand-mère",     WordSwedish="mormor",         WordTurkish="büyükanne",       Phonetic="DYOOSH-ya", ExampleSentence="Gjyshja bën byrek.",             DifficultyScore=2 },
            new() { LessonId=lesson1.Id, WordAlbanian="dera",     WordEnglish="the door",     WordGerman="die Tür",        WordItalian="la porta",      WordFrench="la porte",       WordSwedish="dörren",         WordTurkish="kap?",            Phonetic="DEH-ra",    ExampleSentence="Dera është e hapur.",            DifficultyScore=1 },
        };

        var vocab2 = new List<VocabularyItem>
        {
            new() { LessonId=lesson2.Id, WordAlbanian="i kuq",    WordEnglish="red",    WordGerman="rot",     WordItalian="rosso",   WordFrench="rouge",  WordSwedish="röd",    WordTurkish="k?rm?z?", Phonetic="ee KOOQ",     ExampleSentence="Molla është e kuqe.",    DifficultyScore=1 },
            new() { LessonId=lesson2.Id, WordAlbanian="i blertë", WordEnglish="green",  WordGerman="grün",    WordItalian="verde",   WordFrench="vert",   WordSwedish="grön",   WordTurkish="ye?il",   Phonetic="ee BLER-tuh", ExampleSentence="Bari është i blertë.",   DifficultyScore=1 },
            new() { LessonId=lesson2.Id, WordAlbanian="i kaltër", WordEnglish="blue",   WordGerman="blau",    WordItalian="blu",     WordFrench="bleu",   WordSwedish="blå",    WordTurkish="mavi",    Phonetic="ee KAL-tur",  ExampleSentence="Qielli është i kaltër.", DifficultyScore=1 },
            new() { LessonId=lesson2.Id, WordAlbanian="i bardhë", WordEnglish="white",  WordGerman="weiß",    WordItalian="bianco",  WordFrench="blanc",  WordSwedish="vit",    WordTurkish="beyaz",   Phonetic="ee BAR-dhuh", ExampleSentence="Bora është e bardhë.",   DifficultyScore=1 },
            new() { LessonId=lesson2.Id, WordAlbanian="i zi",     WordEnglish="black",  WordGerman="schwarz", WordItalian="nero",    WordFrench="noir",   WordSwedish="svart",  WordTurkish="siyah",   Phonetic="ee ZEE",      ExampleSentence="Nata është e zezë.",     DifficultyScore=1 },
            new() { LessonId=lesson2.Id, WordAlbanian="i verdhë", WordEnglish="yellow", WordGerman="gelb",    WordItalian="giallo",  WordFrench="jaune",  WordSwedish="gul",    WordTurkish="sar?",    Phonetic="ee VER-dhuh", ExampleSentence="Dielli është i verdhë.", DifficultyScore=1 },
        };

        var vocab3 = new List<VocabularyItem>
        {
            new() { LessonId=lesson3.Id, WordAlbanian="shqiponja", WordEnglish="eagle",       WordGerman="Adler",       WordItalian="aquila",   WordFrench="aigle",     WordSwedish="örn",      WordTurkish="kartal",    Phonetic="shchi-PON-ya", ExampleSentence="Shqiponja fluturon lart.", DifficultyScore=2 },
            new() { LessonId=lesson3.Id, WordAlbanian="mali",      WordEnglish="the mountain",WordGerman="der Berg",    WordItalian="la montagna",WordFrench="la montagne",WordSwedish="berget",  WordTurkish="da?",       Phonetic="MA-lee",       ExampleSentence="Mali është i lartë.",     DifficultyScore=1 },
            new() { LessonId=lesson3.Id, WordAlbanian="lumi",      WordEnglish="the river",   WordGerman="der Fluss",   WordItalian="il fiume",  WordFrench="la rivière", WordSwedish="floden",   WordTurkish="nehir",     Phonetic="LOO-mee",      ExampleSentence="Lumi rrjedh ngadalë.",    DifficultyScore=1 },
            new() { LessonId=lesson3.Id, WordAlbanian="pylli",     WordEnglish="the forest",  WordGerman="der Wald",    WordItalian="la foresta",WordFrench="la forêt",   WordSwedish="skogen",   WordTurkish="orman",     Phonetic="PYUL-lee",     ExampleSentence="Pylli është i gjelbër.", DifficultyScore=2 },
            new() { LessonId=lesson3.Id, WordAlbanian="qeni",      WordEnglish="the dog",     WordGerman="der Hund",    WordItalian="il cane",   WordFrench="le chien",   WordSwedish="hunden",   WordTurkish="köpek",     Phonetic="QYEH-nee",     ExampleSentence="Qeni im është i zgjuar.",DifficultyScore=1 },
            new() { LessonId=lesson3.Id, WordAlbanian="macja",     WordEnglish="the cat",     WordGerman="die Katze",   WordItalian="il gatto",  WordFrench="le chat",    WordSwedish="katten",   WordTurkish="kedi",      Phonetic="MA-tsya",      ExampleSentence="Macja fle gjatë ditës.", DifficultyScore=1 },
        };

        var vocab4 = new List<VocabularyItem>
        {
            new() { LessonId=lesson4.Id, WordAlbanian="buka",     WordEnglish="the bread", WordGerman="das Brot",    WordItalian="il pane",   WordFrench="le pain",    WordSwedish="brödet",   WordTurkish="ekmek",     Phonetic="BOO-ka",        ExampleSentence="Buka është e nxehtë.",              DifficultyScore=1 },
            new() { LessonId=lesson4.Id, WordAlbanian="uji",      WordEnglish="the water", WordGerman="das Wasser",  WordItalian="l'acqua",   WordFrench="l'eau",      WordSwedish="vattnet",  WordTurkish="su",        Phonetic="OO-yee",        ExampleSentence="Uji është i ftohtë.",               DifficultyScore=1 },
            new() { LessonId=lesson4.Id, WordAlbanian="qumështi", WordEnglish="the milk",  WordGerman="die Milch",   WordItalian="il latte",  WordFrench="le lait",    WordSwedish="mjölken",  WordTurkish="süt",       Phonetic="choo-MESH-tee", ExampleSentence="Qumështi është i bardhë.",          DifficultyScore=2 },
            new() { LessonId=lesson4.Id, WordAlbanian="molla",    WordEnglish="the apple", WordGerman="der Apfel",   WordItalian="la mela",   WordFrench="la pomme",   WordSwedish="äpplet",   WordTurkish="elma",      Phonetic="MOL-la",        ExampleSentence="Molla është e kuqe dhe e ëmbël.",   DifficultyScore=1 },
            new() { LessonId=lesson4.Id, WordAlbanian="byreku",   WordEnglish="the börek", WordGerman="das Börek",   WordItalian="il börek",  WordFrench="le börek",   WordSwedish="börek",    WordTurkish="börek",     Phonetic="BYU-re-koo",    ExampleSentence="Byreku i gjyshes është shumë i mirë.", DifficultyScore=2 },
        };

        await context.VocabularyItems.AddRangeAsync(vocab1);
        await context.VocabularyItems.AddRangeAsync(vocab2);
        await context.VocabularyItems.AddRangeAsync(vocab3);
        await context.VocabularyItems.AddRangeAsync(vocab4);
        if (!await context.Badges.AnyAsync())
        {
            var badges = new List<Badge>
    {
        new() { Name="Mësimi i Parë", Description="Përfundove mësimin e parë!",  IconUrl="??", Category="Lessons", CriteriaJson="{}" },
        new() { Name="5 Mësime",      Description="Përfundove 5 mësime!",        IconUrl="??", Category="Lessons", CriteriaJson="{}" },
        new() { Name="10 Mësime",     Description="Përfundove 10 mësime!",       IconUrl="??", Category="Lessons", CriteriaJson="{}" },
        new() { Name="3 Ditë",        Description="3 ditë radhazi!",             IconUrl="??", Category="Streak",  CriteriaJson="{}" },
        new() { Name="7 Ditë",        Description="Javë e plotë!",               IconUrl="?", Category="Streak",  CriteriaJson="{}" },
        new() { Name="30 Ditë",       Description="Muaj i plotë!",               IconUrl="??", Category="Streak",  CriteriaJson="{}" },
        new() { Name="50 Pikë",       Description="Ke fituar 50 pikë!",          IconUrl="?", Category="Points",  CriteriaJson="{}" },
        new() { Name="200 Pikë",      Description="Ke fituar 200 pikë!",         IconUrl="??", Category="Points",  CriteriaJson="{}" },
        new() { Name="500 Pikë",      Description="Ke fituar 500 pikë!",         IconUrl="??", Category="Points",  CriteriaJson="{}" },
        new() { Name="Mesatar",       Description="Ke arritur nivelin Mesatar!",  IconUrl="??", Category="Level",   CriteriaJson="{}" },
        new() { Name="Avancuar",      Description="Ke arritur nivelin Avancuar!", IconUrl="??", Category="Level",   CriteriaJson="{}" },
    };
            await context.Badges.AddRangeAsync(badges);
            await context.SaveChangesAsync();
        }
        await context.SaveChangesAsync();
    }
}