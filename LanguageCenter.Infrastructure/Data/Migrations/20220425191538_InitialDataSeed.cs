using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanguageCenter.Infrastructure.Data.Migrations
{
    public partial class InitialDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "04507fe5-8edf-4348-b7f2-4b78a2136cf8", "Greek", "GREEK" },
                    { "7062a927-849c-4145-8d60-463ca44d72c9", "Italian", "ITALIAN" },
                    { "8ead3f2d-c261-4aa0-b0cd-ab0b8f4c599a", "German", "GERMAN" },
                    { "906c70c9-ba85-45a2-a21e-9e3b1500fb16", "French", "FRENCH" },
                    { "ab32b9ec-6e77-465f-86ff-e9c4a7a296ea", "English", "ENGLISH" },
                    { "d920c0a9-f0a1-4174-92bd-f2310f65d989", "Spanish", "SPANISH" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Capacity", "Description", "DurationInMonths", "EndDate", "LanguageId", "Level", "StartDate", "TeacherId", "Title" },
                values: new object[,]
                {
                    { "16b7802b-c7eb-4fa7-87ab-b1ff10838b11", (short)12, "Настоящия курс, ще ви помогне да развиете вашите езикови познания. Курса се фокусира върху пресъздаване естествената среда на общуване и поставят акцент върху практическата употреба на езика.", (short)3, new DateTime(2022, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "ab32b9ec-6e77-465f-86ff-e9c4a7a296ea", "B1", new DateTime(2022, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Английски за напреднали" },
                    { "3999db9d-6001-440e-a592-99501ec98ceb", (short)12, "Настоящия курс, ще ви помогне да развиете вашите езикови познания. Курса се фокусира върху пресъздаване естествената среда на общуване и поставят акцент върху практическата употреба на езика.", (short)4, new DateTime(2022, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "8ead3f2d-c261-4aa0-b0cd-ab0b8f4c599a", "B1", new DateTime(2022, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Немски за напреднали" },
                    { "3a7d3e93-8ae8-4ff8-8f7b-be355762f5c8", (short)12, "Настоящия курс, ще ви даде базови езикови познания. Започвайки с изучаване на азбуката, трите основки групи езикови времена и други базови езикови познания.", (short)3, new DateTime(2022, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "906c70c9-ba85-45a2-a21e-9e3b1500fb16", "A1", new DateTime(2022, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Френски за начинаещи" },
                    { "4421015c-fdca-4cf6-8927-b1a95a6424d6", (short)12, "Настоящия курс, ще ви даде базови езикови познания. Започвайки с изучаване на азбуката, трите основки групи езикови времена и други базови езикови познания.", (short)4, new DateTime(2022, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "d920c0a9-f0a1-4174-92bd-f2310f65d989", "A1", new DateTime(2022, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Испански за начинаещи" },
                    { "493c4e29-e1ea-43ae-a3b7-10cc7d0b7afc", (short)12, "Настоящия курс, ще ви даде базови езикови познания. Започвайки с изучаване на азбуката, трите основки групи езикови времена и други базови езикови познания.", (short)3, new DateTime(2022, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "ab32b9ec-6e77-465f-86ff-e9c4a7a296ea", "A1", new DateTime(2022, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Английски за начинаещи" },
                    { "5a4c0357-fa81-4a75-af64-b83cb7167eb3", (short)12, "Настоящия курс, ще ви даде базови езикови познания. Започвайки с изучаване на азбуката, трите основки групи езикови времена и други базови езикови познания.", (short)3, new DateTime(2022, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "906c70c9-ba85-45a2-a21e-9e3b1500fb16", "C1", new DateTime(2022, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Професионален френски" },
                    { "61c3d3e4-295d-4b0c-9973-7e2c4bda6066", (short)12, "Настоящия курс, се фокусира върху усъвършенстване на вашите езикови уменя, пресъздава естествената среда на общуване и поставят акцент върху практическата употреба на езика.Основната цел на курса е активирането и развиването на четирите основни езикови умения (четене, писане, слушане и говорене).", (short)3, new DateTime(2022, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "ab32b9ec-6e77-465f-86ff-e9c4a7a296ea", "C1", new DateTime(2022, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Професионален английски" },
                    { "90abf697-d492-4786-af2f-0d6089be2994", (short)12, "Настоящия курс, се фокусира върху усъвършенстване на вашите езикови уменя, пресъздава естествената среда на общуване и поставят акцент върху практическата употреба на езика.Основната цел на курса е активирането и развиването на четирите основни езикови умения (четене, писане, слушане и говорене).", (short)3, new DateTime(2022, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "906c70c9-ba85-45a2-a21e-9e3b1500fb16", "B1", new DateTime(2022, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Френски за напреднали" },
                    { "b46618ac-1804-4445-801b-5f384948d429", (short)12, "Настоящия курс, ще ви даде базови езикови познания. Започвайки с изучаване на азбуката, трите основки групи езикови времена и други базови езикови познания.", (short)3, new DateTime(2022, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "906c70c9-ba85-45a2-a21e-9e3b1500fb16", "C1", new DateTime(2022, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Професионален френски" },
                    { "b6af2168-5a09-4560-879b-986a2290517c", (short)12, "Настоящия курс, се фокусира върху усъвършенстване на вашите езикови уменя, пресъздава естествената среда на общуване и поставят акцент върху практическата употреба на езика.Основната цел на курса е активирането и развиването на четирите основни езикови умения (четене, писане, слушане и говорене).", (short)4, new DateTime(2022, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "d920c0a9-f0a1-4174-92bd-f2310f65d989", "C1", new DateTime(2022, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Професионален испански" },
                    { "bd9b882f-4827-4295-89e5-25cb644b10c9", (short)12, "Настоящия курс, ще ви даде базови езикови познания. Започвайки с изучаване на азбуката, трите основки групи езикови времена и други базови езикови познания.", (short)4, new DateTime(2022, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "8ead3f2d-c261-4aa0-b0cd-ab0b8f4c599a", "A1", new DateTime(2022, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Немски за начинаещи" },
                    { "bdbace89-cc4c-46d1-80fb-85704fc3297f", (short)12, "Настоящия курс, ще ви даде базови езикови познания. Започвайки с изучаване на азбуката, трите основки групи езикови времена и други базови езикови познания.", (short)4, new DateTime(2022, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "7062a927-849c-4145-8d60-463ca44d72c9", "A1", new DateTime(2022, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Италиански за начинаещи" },
                    { "caed6960-44a7-4916-8461-3e89710de76d", (short)12, "Настоящия курс, се фокусира върху усъвършенстване на вашите езикови уменя, пресъздава естествената среда на общуване и поставят акцент върху практическата употреба на езика.Основната цел на курса е активирането и развиването на четирите основни езикови умения (четене, писане, слушане и говорене).", (short)5, new DateTime(2022, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "8ead3f2d-c261-4aa0-b0cd-ab0b8f4c599a", "C1", new DateTime(2022, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Професионален немски" },
                    { "dda416a4-f7fe-424d-b949-41b383b870ba", (short)12, "Настоящия курс, ще ви помогне да развиете вашите езикови познания. Курса се фокусира върху пресъздаване естествената среда на общуване и поставят акцент върху практическата употреба на езика", (short)4, new DateTime(2022, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "7062a927-849c-4145-8d60-463ca44d72c9", "B1", new DateTime(2022, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Италиански за напреднали" },
                    { "e6faff72-790b-43e0-ab32-777db89e2008", (short)12, "Настоящия курс, се фокусира върху усъвършенстване на вашите езикови уменя, пресъздава естествената среда на общуване и поставят акцент върху практическата употреба на езика.Основната цел на курса е активирането и развиването на четирите основни езикови умения (четене, писане, слушане и говорене).", (short)4, new DateTime(2022, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "7062a927-849c-4145-8d60-463ca44d72c9", "C1", new DateTime(2022, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Професионален италиански" },
                    { "ee6128a6-c014-4678-9688-51268f57550e", (short)12, "Настоящия курс, ще ви помогне да развиете вашите езикови познания. Курса се фокусира върху пресъздаване естествената среда на общуване и поставят акцент върху практическата употреба на езика.", (short)4, new DateTime(2022, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "d920c0a9-f0a1-4174-92bd-f2310f65d989", "B1", new DateTime(2022, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Испански за напреднали" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "16b7802b-c7eb-4fa7-87ab-b1ff10838b11");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "3999db9d-6001-440e-a592-99501ec98ceb");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "3a7d3e93-8ae8-4ff8-8f7b-be355762f5c8");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "4421015c-fdca-4cf6-8927-b1a95a6424d6");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "493c4e29-e1ea-43ae-a3b7-10cc7d0b7afc");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "5a4c0357-fa81-4a75-af64-b83cb7167eb3");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "61c3d3e4-295d-4b0c-9973-7e2c4bda6066");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "90abf697-d492-4786-af2f-0d6089be2994");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "b46618ac-1804-4445-801b-5f384948d429");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "b6af2168-5a09-4560-879b-986a2290517c");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "bd9b882f-4827-4295-89e5-25cb644b10c9");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "bdbace89-cc4c-46d1-80fb-85704fc3297f");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "caed6960-44a7-4916-8461-3e89710de76d");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "dda416a4-f7fe-424d-b949-41b383b870ba");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "e6faff72-790b-43e0-ab32-777db89e2008");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "ee6128a6-c014-4678-9688-51268f57550e");

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: "04507fe5-8edf-4348-b7f2-4b78a2136cf8");

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: "7062a927-849c-4145-8d60-463ca44d72c9");

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: "8ead3f2d-c261-4aa0-b0cd-ab0b8f4c599a");

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: "906c70c9-ba85-45a2-a21e-9e3b1500fb16");

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: "ab32b9ec-6e77-465f-86ff-e9c4a7a296ea");

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: "d920c0a9-f0a1-4174-92bd-f2310f65d989");
        }
    }
}
