using MesoShqip.Application.Interfaces;
using MesoShqip.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace MesoShqip.Infrastructure.Services.AI;

public class StoryGeneratorService : IStoryGeneratorService
{
    private readonly ILogger<StoryGeneratorService> _logger;

    public StoryGeneratorService(ILogger<StoryGeneratorService> logger)
    {
        _logger = logger;
    }

    public Task<StoryGenerationResult> GenerateAsync(
        LanguageLevel level,
        string nativeLanguage,
        IReadOnlyList<string> wordsToIntroduce,
        CancellationToken ct = default)
    {
        var stories = GetStories(level, nativeLanguage);
        var random = new Random();
        var story = stories[random.Next(stories.Count)];

        _logger.LogInformation("Mock story generated for level {Level}, language {Lang}", level, nativeLanguage);

        return Task.FromResult(story);
    }

    private static List<StoryGenerationResult> GetStories(LanguageLevel level, string lang)
    {
        return level switch
        {
            LanguageLevel.Fillestor => GetFillestor(lang),
            LanguageLevel.Mesatar => GetMesatar(lang),
            LanguageLevel.Avancuar => GetAvancuar(lang),
            _ => GetFillestor(lang)
        };
    }

    private static List<StoryGenerationResult> GetFillestor(string lang) => new()
    {
        new StoryGenerationResult(
            "Shqiponja e Vogël",
            "Njëherë e një kohë, në malin e lartë, jetonte një shqiponjë e vogël. " +
            "Ajo kishte krahë të bardhë dhe sy të zinj. " +
            "Çdo ditë ajo fluturonte lart dhe shihte shtëpitë e vogla poshtë. " +
            "Babai i saj i thoshte: 'Ti je e lirë si era!' " +
            "Shqiponja ishte shumë e lumtur.",
            GetTranslation("shqiponja_e_vogel", lang),
            new List<NewWordEntry>
            {
                new("shqiponjë", GetWord("eagle", lang), "shchi-PON-ya"),
                new("krahë",     GetWord("wings", lang), "KRA-hë"),
                new("lirë",      GetWord("free", lang),  "LEE-rë"),
                new("mal",       GetWord("mountain", lang), "mal"),
                new("fluturon",  GetWord("flies", lang), "flu-tu-RON"),
            }),

        new StoryGenerationResult(
            "Familja e Lumtur",
            "Arbëri jetonte me familjen e tij në një shtëpi të bukur. " +
            "Nëna gatonte byrek çdo të diel. " +
            "Babai punonte në qytet. " +
            "Motra e vogël luante në oborr. " +
            "Ata ishin shumë të lumtur së bashku.",
            GetTranslation("familja_e_lumtur", lang),
            new List<NewWordEntry>
            {
                new("shtëpi",  GetWord("house", lang),   "shTEH-pi"),
                new("nënë",    GetWord("mother", lang),  "NUH-në"),
                new("babalë",  GetWord("father", lang),  "BA-ba-lë"),
                new("motër",   GetWord("sister", lang),  "MOT-ër"),
                new("lumtur",  GetWord("happy", lang),   "LUM-tur"),
            }),

        new StoryGenerationResult(
            "Macja dhe Qeni",
            "Lumi kishte një macja të bardhë dhe një qen të zi. " +
            "Macja flinte gjatë ditës. " +
            "Qeni luante në oborr. " +
            "Një ditë ata u bënë miq të mirë. " +
            "Tashmë luanin së bashku çdo ditë.",
            GetTranslation("macja_dhe_qeni", lang),
            new List<NewWordEntry>
            {
                new("macja",  GetWord("cat", lang),      "MA-tsya"),
                new("qeni",   GetWord("dog", lang),      "QYEH-ni"),
                new("bardhë", GetWord("white", lang),    "BAR-dhë"),
                new("zi",     GetWord("black", lang),    "zi"),
                new("miq",    GetWord("friends", lang),  "miq"),
            }),
    };

    private static List<StoryGenerationResult> GetMesatar(string lang) => new()
    {
        new StoryGenerationResult(
            "Udha për në Shkodër",
            "Elsa dhe familja e saj vendosën të bënin një udhëtim të gjatë drejt Shkodrës. " +
            "Qyteti i lashtë kishte kalaja të famshme dhe liqenin e bukur. " +
            "Gjatë rrugës, ata kaluan nëpër fusha të gjelbra dhe male të larta. " +
            "Elsa fotografonte gjithçka me aparatin e saj të ri. " +
            "Kur arritën, gjyshja i priste me byrek të nxehtë dhe dashamirësi të madhe.",
            GetTranslation("udha_per_ne_shkoder", lang),
            new List<NewWordEntry>
            {
                new("udhëtim",     GetWord("journey", lang),    "u-dhë-TIM"),
                new("kala",        GetWord("castle", lang),     "KA-la"),
                new("liqen",       GetWord("lake", lang),       "li-QYEN"),
                new("dashamirësi", GetWord("kindness", lang),   "da-sha-mi-RË-si"),
                new("fotografonte",GetWord("photographed", lang),"fo-to-gra-FON-te"),
            }),

        new StoryGenerationResult(
            "Mësuesi i Mirë",
            "Zana ishte mësuese në një shkollë të vogël në fshat. " +
            "Ajo i donte nxënësit e saj shumë dhe punonte me zell çdo ditë. " +
            "Nxënësit mësonin shqip, matematikë dhe histori. " +
            "Çdo të premte, Zana tregonte histori interesante për Shqipërinë. " +
            "Nxënësit e prisnin me padurim orën e historisë.",
            GetTranslation("mesuesi_i_mire", lang),
            new List<NewWordEntry>
            {
                new("mësuese",  GetWord("teacher", lang),   "më-SU-e-se"),
                new("nxënës",   GetWord("student", lang),   "n-XË-nës"),
                new("zell",     GetWord("diligence", lang), "zell"),
                new("padurim",  GetWord("impatience", lang),"pa-DU-rim"),
                new("histori",  GetWord("history", lang),   "his-TO-ri"),
            }),
    };

    private static List<StoryGenerationResult> GetAvancuar(string lang) => new()
    {
        new StoryGenerationResult(
            "Trashëgimia Shqiptare",
            "Në zemër të Ballkanit, midis maleve madhështore dhe detit të kaltër, " +
            "shtrihet Shqipëria — vendi i shqiponjave dhe i traditave të lashta. " +
            "Populli shqiptar ka ruajtur me krenari gjuhën e tij për mijëra vjet, " +
            "pavarësisht pushtimeve dhe sfidave të historisë. " +
            "Gjuha shqipe, me dialektet e saj gegë dhe toskë, " +
            "mbetet simbol i identitetit dhe i rezistencës kombëtare. " +
            "Çdo fjalë e kësaj gjuhe mbart kujtimet e brezave të tërë.",
            GetTranslation("trashegimia_shqiptare", lang),
            new List<NewWordEntry>
            {
                new("trashëgimi",  GetWord("heritage", lang),   "tra-shë-GI-mi"),
                new("madhështor",  GetWord("magnificent", lang),"mad-hësh-TOR"),
                new("krenari",     GetWord("pride", lang),      "kre-NA-ri"),
                new("rezistencë",  GetWord("resistance", lang), "re-zis-TEN-cë"),
                new("identitet",   GetWord("identity", lang),   "i-den-ti-TET"),
            }),
    };

    private static string GetTranslation(string storyKey, string lang)
    {
        var translations = new Dictionary<string, Dictionary<string, string>>
        {
            ["shqiponja_e_vogel"] = new()
            {
                ["en"] = "Once upon a time, on a high mountain, lived a small eagle. She had white wings and black eyes. Every day she flew high and saw the small houses below. Her father told her: 'You are free as the wind!' The eagle was very happy.",
                ["de"] = "Es war einmal auf einem hohen Berg ein kleiner Adler. Sie hatte weiße Flügel und schwarze Augen. Jeden Tag flog sie hoch und sah die kleinen Häuser unten. Ihr Vater sagte ihr: 'Du bist frei wie der Wind!' Der Adler war sehr glücklich.",
                ["it"] = "C'era una volta, su un alto monte, una piccola aquila. Aveva ali bianche e occhi neri. Ogni giorno volava in alto e vedeva le piccole case in basso. Suo padre le diceva: 'Sei libera come il vento!' L'aquila era molto felice.",
                ["fr"] = "Il était une fois, sur une haute montagne, vivait un petit aigle. Elle avait des ailes blanches et des yeux noirs. Chaque jour elle volait haut et voyait les petites maisons en bas. Son père lui disait: 'Tu es libre comme le vent!' L'aigle était très heureuse.",
                ["sv"] = "Det var en gång, på ett högt berg, en liten örn. Hon hade vita vingar och svarta ögon. Varje dag flög hon högt och såg de små husen nedan. Hennes far sa till henne: 'Du är fri som vinden!' Örnen var mycket glad.",
                ["tr"] = "Bir zamanlar, yüksek bir da?da, küçük bir kartal ya?ard?. Beyaz kanatlar? ve siyah gözleri vard?. Her gün yüksekte uçar ve a?a??daki küçük evleri görürdü. Babas? ona derdi: 'Sen rüzgar gibi özgürsün!' Kartal çok mutluydu.",
            },
            ["familja_e_lumtur"] = new()
            {
                ["en"] = "Arbër lived with his family in a beautiful house. Mom cooked byrek every Sunday. Dad worked in the city. The little sister played in the yard. They were very happy together.",
                ["de"] = "Arbër lebte mit seiner Familie in einem schönen Haus. Mama kochte jeden Sonntag Byrek. Papa arbeitete in der Stadt. Die kleine Schwester spielte im Hof. Sie waren sehr glücklich zusammen.",
                ["it"] = "Arbër viveva con la sua famiglia in una bella casa. La mamma cucinava byrek ogni domenica. Il papà lavorava in città. La sorellina giocava nel cortile. Erano molto felici insieme.",
                ["fr"] = "Arbër vivait avec sa famille dans une belle maison. Maman cuisinait du byrek chaque dimanche. Papa travaillait en ville. La petite sœur jouait dans la cour. Ils étaient très heureux ensemble.",
                ["sv"] = "Arbër bodde med sin familj i ett vackert hus. Mamma lagade byrek varje söndag. Pappa jobbade i staden. Lillasystern lekte på gården. De var väldigt lyckliga tillsammans.",
                ["tr"] = "Arbër ailesiyle güzel bir evde ya??yordu. Anne her pazar byrek pi?irirdi. Baba ?ehirde çal???rd?. Küçük k?z karde? bahçede oynard?. Birlikte çok mutluydular.",
            },
            ["macja_dhe_qeni"] = new()
            {
                ["en"] = "Lumi had a white cat and a black dog. The cat slept during the day. The dog played in the yard. One day they became good friends. Now they played together every day.",
                ["de"] = "Lumi hatte eine weiße Katze und einen schwarzen Hund. Die Katze schlief tagsüber. Der Hund spielte im Hof. Eines Tages wurden sie gute Freunde. Jetzt spielten sie jeden Tag zusammen.",
                ["it"] = "Lumi aveva un gatto bianco e un cane nero. Il gatto dormiva durante il giorno. Il cane giocava nel cortile. Un giorno diventarono buoni amici. Ora giocavano insieme ogni giorno.",
                ["fr"] = "Lumi avait un chat blanc et un chien noir. Le chat dormait pendant la journée. Le chien jouait dans la cour. Un jour ils devinrent bons amis. Maintenant ils jouaient ensemble chaque jour.",
                ["sv"] = "Lumi hade en vit katt och en svart hund. Katten sov på dagen. Hunden lekte på gården. En dag blev de goda vänner. Nu lekte de tillsammans varje dag.",
                ["tr"] = "Lumi'nin beyaz bir kedisi ve siyah bir köpe?i vard?. Kedi gündüzleri uyurdu. Köpek bahçede oynard?. Bir gün iyi arkada? oldular. Art?k her gün birlikte oynuyorlard?.",
            },
            ["udha_per_ne_shkoder"] = new()
            {
                ["en"] = "Elsa and her family decided to take a long trip to Shkodra. The ancient city had famous castles and a beautiful lake. Along the way, they passed through green fields and high mountains. Elsa photographed everything with her new camera. When they arrived, grandma was waiting with hot byrek and great warmth.",
                ["de"] = "Elsa und ihre Familie beschlossen, eine lange Reise nach Shkodra zu machen. Die alte Stadt hatte berühmte Burgen und einen schönen See. Unterwegs fuhren sie durch grüne Felder und hohe Berge. Elsa fotografierte alles mit ihrer neuen Kamera. Als sie ankamen, wartete die Großmutter mit heißem Byrek und großer Herzlichkeit.",
                ["it"] = "Elsa e la sua famiglia decisero di fare un lungo viaggio a Scutari. La città antica aveva castelli famosi e un bel lago. Per strada, passarono attraverso campi verdi e alte montagne. Elsa fotografava tutto con la sua nuova macchina fotografica. Quando arrivarono, la nonna li aspettava con byrek caldo e grande calore.",
                ["fr"] = "Elsa et sa famille décidèrent de faire un long voyage à Shkodra. La vieille ville avait des châteaux célèbres et un beau lac. En chemin, ils traversèrent des champs verts et de hautes montagnes. Elsa photographiait tout avec son nouvel appareil photo. Quand ils arrivèrent, grand-mère les attendait avec du byrek chaud et une grande chaleur.",
                ["sv"] = "Elsa och hennes familj bestämde sig för att göra en lång resa till Shkodra. Den gamla staden hade berömda slott och en vacker sjö. På vägen passerade de gröna fält och höga berg. Elsa fotograferade allt med sin nya kamera. När de kom fram väntade farmor med varm byrek och stor värme.",
                ["tr"] = "Elsa ve ailesi ??kodra'ya uzun bir yolculuk yapmaya karar verdiler. Antik ?ehrin ünlü kaleleri ve güzel bir gölü vard?. Yol boyunca ye?il tarlalar ve yüksek da?lardan geçtiler. Elsa yeni kameras?yla her ?eyi foto?raflad?. Vard?klar?nda büyükanne s?cak byrek ve büyük bir s?cakl?kla bekliyordu.",
            },
            ["mesuesi_i_mire"] = new()
            {
                ["en"] = "Zana was a teacher in a small village school. She loved her students very much and worked diligently every day. The students learned Albanian, mathematics and history. Every Friday, Zana told interesting stories about Albania. The students eagerly awaited the history lesson.",
                ["de"] = "Zana war Lehrerin an einer kleinen Dorfschule. Sie liebte ihre Schüler sehr und arbeitete jeden Tag fleißig. Die Schüler lernten Albanisch, Mathematik und Geschichte. Jeden Freitag erzählte Zana interessante Geschichten über Albanien. Die Schüler warteten ungeduldig auf die Geschichtsstunde.",
                ["it"] = "Zana era insegnante in una piccola scuola di villaggio. Amava molto i suoi studenti e lavorava diligentemente ogni giorno. Gli studenti imparavano l'albanese, la matematica e la storia. Ogni venerdì, Zana raccontava storie interessanti sull'Albania. Gli studenti aspettavano con impazienza l'ora di storia.",
                ["fr"] = "Zana était enseignante dans une petite école de village. Elle aimait beaucoup ses élèves et travaillait avec zèle chaque jour. Les élèves apprenaient l'albanais, les mathématiques et l'histoire. Chaque vendredi, Zana racontait des histoires intéressantes sur l'Albanie. Les élèves attendaient avec impatience le cours d'histoire.",
                ["sv"] = "Zana var lärare på en liten byskola. Hon älskade sina elever mycket och jobbade flitigt varje dag. Eleverna lärde sig albanska, matematik och historia. Varje fredag berättade Zana intressanta historier om Albanien. Eleverna väntade ivrigt på historielektionen.",
                ["tr"] = "Zana küçük bir köy okulunda ö?retmendi. Ö?rencilerini çok sever ve her gün çal??kanl?kla çal???rd?. Ö?renciler Arnavutça, matematik ve tarih ö?reniyordu. Her Cuma, Zana Arnavutluk hakk?nda ilginç hikayeler anlat?rd?. Ö?renciler tarih dersini sab?rs?zl?kla bekliyordu.",
            },
            ["trashegimia_shqiptare"] = new()
            {
                ["en"] = "In the heart of the Balkans, between magnificent mountains and the blue sea, lies Albania — the land of eagles and ancient traditions. The Albanian people have proudly preserved their language for thousands of years, despite conquests and historical challenges. The Albanian language, with its Geg and Tosk dialects, remains a symbol of identity and national resistance. Every word of this language carries the memories of entire generations.",
                ["de"] = "Im Herzen des Balkans, zwischen majestätischen Bergen und dem blauen Meer, liegt Albanien — das Land der Adler und der alten Traditionen. Das albanische Volk hat seine Sprache trotz Eroberungen und historischer Herausforderungen seit Jahrtausenden stolz bewahrt. Die albanische Sprache mit ihren Dialekten Gegisch und Toskisch bleibt ein Symbol der Identität und des nationalen Widerstands. Jedes Wort dieser Sprache trägt die Erinnerungen ganzer Generationen.",
                ["it"] = "Nel cuore dei Balcani, tra magnifiche montagne e il mare azzurro, si trova l'Albania — la terra delle aquile e delle antiche tradizioni. Il popolo albanese ha preservato con orgoglio la sua lingua per migliaia di anni, nonostante le conquiste e le sfide storiche. La lingua albanese, con i suoi dialetti ghego e tosco, rimane un simbolo di identità e resistenza nazionale. Ogni parola di questa lingua porta i ricordi di intere generazioni.",
                ["fr"] = "Au cœur des Balkans, entre de magnifiques montagnes et la mer bleue, se trouve l'Albanie — le pays des aigles et des traditions anciennes. Le peuple albanais a fièrement préservé sa langue pendant des millénaires, malgré les conquêtes et les défis historiques. La langue albanaise, avec ses dialectes guègue et tosque, reste un symbole d'identité et de résistance nationale. Chaque mot de cette langue porte les souvenirs de générations entières.",
                ["sv"] = "I hjärtat av Balkan, mellan magnifika berg och det blå havet, ligger Albanien — örnarnas land och de gamla traditionernas hemland. Det albanska folket har stolt bevarat sitt språk i tusentals år, trots erövningar och historiska utmaningar. Det albanska språket, med sina geg- och toskdialekter, förblir en symbol för identitet och nationellt motstånd. Varje ord i detta språk bär minnen av hela generationer.",
                ["tr"] = "Balkanlar?n kalbinde, muhte?em da?lar ve mavi deniz aras?nda Arnavutluk uzan?r — kartallar?n ve kadim geleneklerin ülkesi. Arnavut halk?, fetihler ve tarihsel zorluklara ra?men dilini binlerce y?l boyunca gururla korumu?tur. Geg ve Tosk lehçeleriyle Arnavutça dili, kimli?in ve ulusal direni?in sembolü olmaya devam etmektedir. Bu dilin her kelimesi nesillerin an?lar?n? ta??r.",
            },
        };

        if (translations.TryGetValue(storyKey, out var langMap))
            return langMap.TryGetValue(lang, out var translation) ? translation : langMap["en"];

        return "Translation not available.";
    }

    private static string GetWord(string concept, string lang)
    {
        var words = new Dictionary<string, Dictionary<string, string>>
        {
            ["eagle"] = new() { ["en"] = "eagle", ["de"] = "Adler", ["it"] = "aquila", ["fr"] = "aigle", ["sv"] = "örn", ["tr"] = "kartal" },
            ["wings"] = new() { ["en"] = "wings", ["de"] = "Flügel", ["it"] = "ali", ["fr"] = "ailes", ["sv"] = "vingar", ["tr"] = "kanatlar" },
            ["free"] = new() { ["en"] = "free", ["de"] = "frei", ["it"] = "libero", ["fr"] = "libre", ["sv"] = "fri", ["tr"] = "özgür" },
            ["mountain"] = new() { ["en"] = "mountain", ["de"] = "Berg", ["it"] = "montagna", ["fr"] = "montagne", ["sv"] = "berg", ["tr"] = "da?" },
            ["flies"] = new() { ["en"] = "flies", ["de"] = "fliegt", ["it"] = "vola", ["fr"] = "vole", ["sv"] = "flyger", ["tr"] = "uçar" },
            ["house"] = new() { ["en"] = "house", ["de"] = "Haus", ["it"] = "casa", ["fr"] = "maison", ["sv"] = "hus", ["tr"] = "ev" },
            ["mother"] = new() { ["en"] = "mother", ["de"] = "Mutter", ["it"] = "madre", ["fr"] = "mère", ["sv"] = "mamma", ["tr"] = "anne" },
            ["father"] = new() { ["en"] = "father", ["de"] = "Vater", ["it"] = "padre", ["fr"] = "père", ["sv"] = "pappa", ["tr"] = "baba" },
            ["sister"] = new() { ["en"] = "sister", ["de"] = "Schwester", ["it"] = "sorella", ["fr"] = "sœur", ["sv"] = "syster", ["tr"] = "k?z karde?" },
            ["happy"] = new() { ["en"] = "happy", ["de"] = "glücklich", ["it"] = "felice", ["fr"] = "heureux", ["sv"] = "lycklig", ["tr"] = "mutlu" },
            ["cat"] = new() { ["en"] = "cat", ["de"] = "Katze", ["it"] = "gatto", ["fr"] = "chat", ["sv"] = "katt", ["tr"] = "kedi" },
            ["dog"] = new() { ["en"] = "dog", ["de"] = "Hund", ["it"] = "cane", ["fr"] = "chien", ["sv"] = "hund", ["tr"] = "köpek" },
            ["white"] = new() { ["en"] = "white", ["de"] = "weiß", ["it"] = "bianco", ["fr"] = "blanc", ["sv"] = "vit", ["tr"] = "beyaz" },
            ["black"] = new() { ["en"] = "black", ["de"] = "schwarz", ["it"] = "nero", ["fr"] = "noir", ["sv"] = "svart", ["tr"] = "siyah" },
            ["friends"] = new() { ["en"] = "friends", ["de"] = "Freunde", ["it"] = "amici", ["fr"] = "amis", ["sv"] = "vänner", ["tr"] = "arkada?lar" },
            ["journey"] = new() { ["en"] = "journey", ["de"] = "Reise", ["it"] = "viaggio", ["fr"] = "voyage", ["sv"] = "resa", ["tr"] = "yolculuk" },
            ["castle"] = new() { ["en"] = "castle", ["de"] = "Burg", ["it"] = "castello", ["fr"] = "château", ["sv"] = "slott", ["tr"] = "kale" },
            ["lake"] = new() { ["en"] = "lake", ["de"] = "See", ["it"] = "lago", ["fr"] = "lac", ["sv"] = "sjö", ["tr"] = "göl" },
            ["kindness"] = new() { ["en"] = "kindness", ["de"] = "Freundlichkeit", ["it"] = "gentilezza", ["fr"] = "gentillesse", ["sv"] = "vänlighet", ["tr"] = "iyilik" },
            ["photographed"] = new() { ["en"] = "photographed", ["de"] = "fotografierte", ["it"] = "fotografava", ["fr"] = "photographiait", ["sv"] = "fotograferade", ["tr"] = "foto?raflad?" },
            ["teacher"] = new() { ["en"] = "teacher", ["de"] = "Lehrerin", ["it"] = "insegnante", ["fr"] = "enseignante", ["sv"] = "lärare", ["tr"] = "ö?retmen" },
            ["student"] = new() { ["en"] = "student", ["de"] = "Schüler", ["it"] = "studente", ["fr"] = "élève", ["sv"] = "elev", ["tr"] = "ö?renci" },
            ["diligence"] = new() { ["en"] = "diligence", ["de"] = "Fleiß", ["it"] = "diligenza", ["fr"] = "zèle", ["sv"] = "flit", ["tr"] = "çal??kanl?k" },
            ["impatience"] = new() { ["en"] = "eagerness", ["de"] = "Ungeduld", ["it"] = "impazienza", ["fr"] = "impatience", ["sv"] = "iver", ["tr"] = "sab?rs?zl?k" },
            ["history"] = new() { ["en"] = "history", ["de"] = "Geschichte", ["it"] = "storia", ["fr"] = "histoire", ["sv"] = "historia", ["tr"] = "tarih" },
            ["heritage"] = new() { ["en"] = "heritage", ["de"] = "Erbe", ["it"] = "eredità", ["fr"] = "patrimoine", ["sv"] = "arv", ["tr"] = "miras" },
            ["magnificent"] = new() { ["en"] = "magnificent", ["de"] = "majestätisch", ["it"] = "magnifico", ["fr"] = "magnifique", ["sv"] = "magnifik", ["tr"] = "muhte?em" },
            ["pride"] = new() { ["en"] = "pride", ["de"] = "Stolz", ["it"] = "orgoglio", ["fr"] = "fierté", ["sv"] = "stolthet", ["tr"] = "gurur" },
            ["resistance"] = new() { ["en"] = "resistance", ["de"] = "Widerstand", ["it"] = "resistenza", ["fr"] = "résistance", ["sv"] = "motstånd", ["tr"] = "direni?" },
            ["identity"] = new() { ["en"] = "identity", ["de"] = "Identität", ["it"] = "identità", ["fr"] = "identité", ["sv"] = "identitet", ["tr"] = "kimlik" },
        };

        if (words.TryGetValue(concept, out var langMap))
            return langMap.TryGetValue(lang, out var word) ? word : langMap["en"];

        return concept;
    }
}