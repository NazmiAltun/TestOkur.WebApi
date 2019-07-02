namespace TestOkur.WebApi.Migrations
{
	using System;
	using Microsoft.EntityFrameworkCore.Migrations;
	using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

	public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "appsettings_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "exam_type_optical_form_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "exam_types_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "exams_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "form_lesson_sections_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "lessons_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "optical_form_definitions_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "optical_form_text_locations_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "optical_form_types_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "sms_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "subjects_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "templates_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "units_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "user_seq",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "answer_form_format",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_answer_form_format", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "appsettings",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    name_value = table.Column<string>(maxLength: 100, nullable: false),
                    value = table.Column<string>(nullable: false),
                    comment = table.Column<string>(nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_on_utc = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<int>(nullable: false),
                    updated_on_utc = table.Column<DateTime>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_appsettings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false),
                    name_value = table.Column<string>(maxLength: 50, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "classrooms",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    grade_value = table.Column<int>(nullable: false),
                    name_value = table.Column<string>(maxLength: 50, nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_on_utc = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<int>(nullable: false),
                    updated_on_utc = table.Column<DateTime>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_classrooms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "directions",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false),
                    name = table.Column<string>(maxLength: 100, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_directions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "exam_booklet_type",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exam_booklet_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "exam_types",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    name_value = table.Column<string>(maxLength: 100, nullable: false),
                    default_incorrect_elimination_rate_value = table.Column<int>(nullable: false),
                    order = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_on_utc = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<int>(nullable: false),
                    updated_on_utc = table.Column<DateTime>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exam_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "formula_types",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false),
                    name = table.Column<string>(maxLength: 20, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_formula_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lessons",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    name_value = table.Column<string>(maxLength: 50, nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_on_utc = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<int>(nullable: false),
                    updated_on_utc = table.Column<DateTime>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lessons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "license_types",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false),
                    name_value = table.Column<string>(maxLength: 150, nullable: false),
                    max_allowed_device_count = table.Column<int>(nullable: false),
                    max_allowed_record_count = table.Column<int>(nullable: false),
                    can_scan = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_license_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "school_types",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false),
                    name = table.Column<string>(maxLength: 100, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_school_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "smses",
                columns: table => new
                {
                    user_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    receiver_value = table.Column<string>(maxLength: 20, nullable: false),
                    body = table.Column<string>(nullable: false),
                    subject = table.Column<string>(nullable: false),
                    successful = table.Column<bool>(nullable: false),
                    result = table.Column<string>(nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_on_utc = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<int>(nullable: false),
                    updated_on_utc = table.Column<DateTime>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_smses", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "templates",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    name_value = table.Column<string>(maxLength: 100, nullable: false),
                    subject = table.Column<string>(nullable: false),
                    path = table.Column<string>(nullable: false),
                    comment = table.Column<string>(nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_on_utc = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<int>(nullable: false),
                    updated_on_utc = table.Column<DateTime>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_templates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "districts",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false),
                    name_value = table.Column<string>(maxLength: 150, nullable: false),
                    city_id = table.Column<long>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_districts", x => x.id);
                    table.ForeignKey(
                        name: "fk_districts_cities_city_id",
                        column: x => x.city_id,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    first_name_value = table.Column<string>(maxLength: 50, nullable: false),
                    last_name_value = table.Column<string>(maxLength: 50, nullable: false),
                    student_number_value = table.Column<int>(nullable: false),
                    classroom_id = table.Column<long>(nullable: true),
                    user_id = table.Column<int>(nullable: false),
                    notes = table.Column<string>(maxLength: 500, nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_on_utc = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<int>(nullable: false),
                    updated_on_utc = table.Column<DateTime>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_students", x => x.id);
                    table.ForeignKey(
                        name: "fk_students_classrooms_classroom_id",
                        column: x => x.classroom_id,
                        principalTable: "classrooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "score_formulas",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name_value = table.Column<string>(maxLength: 20, nullable: false),
                    grade_value = table.Column<int>(nullable: false),
                    base_point = table.Column<float>(nullable: false),
                    formula_type_id = table.Column<long>(nullable: true),
                    user_id = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_on_utc = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<int>(nullable: false),
                    updated_on_utc = table.Column<DateTime>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_score_formulas", x => x.id);
                    table.ForeignKey(
                        name: "fk_score_formulas_formula_types_formula_type_id",
                        column: x => x.formula_type_id,
                        principalTable: "formula_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "exams",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    answer_form_format_id = table.Column<long>(nullable: true),
                    user_id = table.Column<int>(nullable: false),
                    name_value = table.Column<string>(maxLength: 150, nullable: false),
                    exam_type_id = table.Column<long>(nullable: true),
                    incorrect_elimination_rate_value = table.Column<int>(nullable: false),
                    notes = table.Column<string>(nullable: true),
                    exam_date = table.Column<DateTime>(nullable: false),
                    applicable_form_type_code = table.Column<string>(nullable: true),
                    exam_booklet_type_id = table.Column<long>(nullable: true),
                    lesson_id = table.Column<long>(nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_on_utc = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<int>(nullable: false),
                    updated_on_utc = table.Column<DateTime>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exams", x => x.id);
                    table.ForeignKey(
                        name: "fk_exams_exam_booklet_type_exam_booklet_type_id",
                        column: x => x.exam_booklet_type_id,
                        principalTable: "exam_booklet_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_exams_answer_form_format_answer_form_format_id",
                        column: x => x.answer_form_format_id,
                        principalTable: "answer_form_format",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_exams_exam_types_exam_type_id",
                        column: x => x.exam_type_id,
                        principalTable: "exam_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_exams_lessons_lesson_id",
                        column: x => x.lesson_id,
                        principalTable: "lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "units",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    user_id = table.Column<int>(nullable: false),
                    name_value = table.Column<string>(maxLength: 150, nullable: false),
                    lesson_id = table.Column<long>(nullable: true),
                    grade_value = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_on_utc = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<int>(nullable: false),
                    updated_on_utc = table.Column<DateTime>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_units", x => x.id);
                    table.ForeignKey(
                        name: "fk_units_lessons_lesson_id",
                        column: x => x.lesson_id,
                        principalTable: "lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "optical_form_types",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    name_value = table.Column<string>(maxLength: 100, nullable: false),
                    school_type_id = table.Column<long>(nullable: true),
                    max_question_count = table.Column<int>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    configuration_file = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_optical_form_types", x => x.id);
                    table.ForeignKey(
                        name: "fk_optical_form_types_school_types_school_type_id",
                        column: x => x.school_type_id,
                        principalTable: "school_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    subject_id = table.Column<string>(nullable: false),
                    sms_balance = table.Column<int>(nullable: false),
                    city_id = table.Column<long>(nullable: true),
                    district_id = table.Column<long>(nullable: true),
                    email_value = table.Column<string>(maxLength: 255, nullable: false),
                    phone_value = table.Column<string>(maxLength: 20, nullable: false),
                    first_name_value = table.Column<string>(maxLength: 50, nullable: false),
                    last_name_value = table.Column<string>(maxLength: 50, nullable: false),
                    school_name_value = table.Column<string>(maxLength: 100, nullable: false),
                    registrar_full_name_value = table.Column<string>(maxLength: 100, nullable: false),
                    registrar_phone_value = table.Column<string>(maxLength: 20, nullable: false),
                    notes = table.Column<string>(maxLength: 500, nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_on_utc = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<int>(nullable: false),
                    updated_on_utc = table.Column<DateTime>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_cities_city_id",
                        column: x => x.city_id,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_users_districts_district_id",
                        column: x => x.district_id,
                        principalTable: "districts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "contacts",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    phone_value = table.Column<string>(maxLength: 20, nullable: false),
                    use_for_sms = table.Column<bool>(nullable: false),
                    student_id = table.Column<long>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contacts", x => x.id);
                    table.ForeignKey(
                        name: "fk_contacts_students_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    name_value = table.Column<string>(maxLength: 150, nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_on_utc = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<int>(nullable: false),
                    updated_on_utc = table.Column<DateTime>(nullable: false),
                    unit_id = table.Column<long>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subjects", x => x.id);
                    table.ForeignKey(
                        name: "fk_subjects_units_unit_id",
                        column: x => x.unit_id,
                        principalTable: "units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exam_type_optical_form_types",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    optical_form_type_id = table.Column<long>(nullable: true),
                    exam_type_id = table.Column<long>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exam_type_optical_form_types", x => x.id);
                    table.ForeignKey(
                        name: "fk_exam_type_optical_form_types_exam_types_exam_type_id",
                        column: x => x.exam_type_id,
                        principalTable: "exam_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_exam_type_optical_form_types_optical_form_types_optical_for~",
                        column: x => x.optical_form_type_id,
                        principalTable: "optical_form_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_lesson_sections",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    lesson_id = table.Column<long>(nullable: true),
                    max_question_count = table.Column<int>(nullable: false),
                    name_tag = table.Column<string>(nullable: true),
                    form_part = table.Column<int>(nullable: false),
                    list_order = table.Column<int>(nullable: false),
                    optical_form_type_id = table.Column<long>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_form_lesson_sections", x => x.id);
                    table.ForeignKey(
                        name: "fk_form_lesson_sections_lessons_lesson_id",
                        column: x => x.lesson_id,
                        principalTable: "lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_form_lesson_sections_optical_form_types_optical_form_type_id",
                        column: x => x.optical_form_type_id,
                        principalTable: "optical_form_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "optical_form_definitions",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    name = table.Column<string>(maxLength: 50, nullable: false),
                    student_no_fill_width = table.Column<int>(nullable: false),
                    student_no_xinterval = table.Column<int>(nullable: false),
                    student_no_yinterval = table.Column<int>(nullable: false),
                    has_box_for_student_number = table.Column<bool>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    path = table.Column<string>(nullable: true),
                    text_direction_id = table.Column<long>(nullable: true),
                    student_number_fill_direction_id = table.Column<long>(nullable: true),
                    school_type_id = table.Column<long>(nullable: true),
                    optical_form_type_id = table.Column<long>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_optical_form_definitions", x => x.id);
                    table.ForeignKey(
                        name: "fk_optical_form_definitions_directions_student_number_fill_dir~",
                        column: x => x.student_number_fill_direction_id,
                        principalTable: "directions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_optical_form_definitions_directions_text_direction_id",
                        column: x => x.text_direction_id,
                        principalTable: "directions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_optical_form_definitions_optical_form_types_optical_form_ty~",
                        column: x => x.optical_form_type_id,
                        principalTable: "optical_form_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_optical_form_definitions_school_types_school_type_id",
                        column: x => x.school_type_id,
                        principalTable: "school_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "lesson_coefficients",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    exam_lesson_section_id = table.Column<long>(nullable: true),
                    coefficient = table.Column<float>(nullable: false),
                    score_formula_id = table.Column<long>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lesson_coefficients", x => x.id);
                    table.ForeignKey(
                        name: "fk_lesson_coefficients_score_formulas_score_formula_id",
                        column: x => x.score_formula_id,
                        principalTable: "score_formulas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_lesson_coefficients_form_lesson_sections_exam_lesson_sectio~",
                        column: x => x.exam_lesson_section_id,
                        principalTable: "form_lesson_sections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "optical_form_text_locations",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    name_x = table.Column<int>(nullable: false),
                    name_y = table.Column<int>(nullable: false),
                    surname_x = table.Column<int>(nullable: false),
                    surname_y = table.Column<int>(nullable: false),
                    class_x = table.Column<int>(nullable: false),
                    class_y = table.Column<int>(nullable: false),
                    student_no_x = table.Column<int>(nullable: false),
                    student_no_y = table.Column<int>(nullable: false),
                    exam_name_x = table.Column<int>(nullable: false),
                    exam_name_y = table.Column<int>(nullable: false),
                    student_no_filling_part_x = table.Column<int>(nullable: false),
                    student_no_filling_part_y = table.Column<int>(nullable: false),
                    optical_form_definition_id = table.Column<long>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_optical_form_text_locations", x => x.id);
                    table.ForeignKey(
                        name: "fk_optical_form_text_locations_optical_form_definitions_optica~",
                        column: x => x.optical_form_definition_id,
                        principalTable: "optical_form_definitions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_contacts_student_id",
                table: "contacts",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "ix_districts_city_id",
                table: "districts",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_exam_type_optical_form_types_exam_type_id",
                table: "exam_type_optical_form_types",
                column: "exam_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_exam_type_optical_form_types_optical_form_type_id",
                table: "exam_type_optical_form_types",
                column: "optical_form_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_exams_exam_booklet_type_id",
                table: "exams",
                column: "exam_booklet_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_exams_answer_form_format_id",
                table: "exams",
                column: "answer_form_format_id");

            migrationBuilder.CreateIndex(
                name: "ix_exams_exam_type_id",
                table: "exams",
                column: "exam_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_exams_lesson_id",
                table: "exams",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "ix_form_lesson_sections_lesson_id",
                table: "form_lesson_sections",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "ix_form_lesson_sections_optical_form_type_id",
                table: "form_lesson_sections",
                column: "optical_form_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_lesson_coefficients_score_formula_id",
                table: "lesson_coefficients",
                column: "score_formula_id");

            migrationBuilder.CreateIndex(
                name: "ix_lesson_coefficients_exam_lesson_section_id",
                table: "lesson_coefficients",
                column: "exam_lesson_section_id");

            migrationBuilder.CreateIndex(
                name: "ix_lessons_user_id",
                table: "lessons",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_optical_form_definitions_student_number_fill_direction_id",
                table: "optical_form_definitions",
                column: "student_number_fill_direction_id");

            migrationBuilder.CreateIndex(
                name: "ix_optical_form_definitions_text_direction_id",
                table: "optical_form_definitions",
                column: "text_direction_id");

            migrationBuilder.CreateIndex(
                name: "ix_optical_form_definitions_optical_form_type_id",
                table: "optical_form_definitions",
                column: "optical_form_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_optical_form_definitions_school_type_id",
                table: "optical_form_definitions",
                column: "school_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_optical_form_text_locations_optical_form_definition_id",
                table: "optical_form_text_locations",
                column: "optical_form_definition_id");

            migrationBuilder.CreateIndex(
                name: "ix_optical_form_types_school_type_id",
                table: "optical_form_types",
                column: "school_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_score_formulas_formula_type_id",
                table: "score_formulas",
                column: "formula_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_students_user_id",
                table: "students",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_students_classroom_id",
                table: "students",
                column: "classroom_id");

            migrationBuilder.CreateIndex(
                name: "ix_subjects_unit_id",
                table: "subjects",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_units_lesson_id",
                table: "units",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "ix_units_user_id",
                table: "units",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_city_id",
                table: "users",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_district_id",
                table: "users",
                column: "district_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appsettings");

            migrationBuilder.DropTable(
                name: "contacts");

            migrationBuilder.DropTable(
                name: "exam_type_optical_form_types");

            migrationBuilder.DropTable(
                name: "exams");

            migrationBuilder.DropTable(
                name: "lesson_coefficients");

            migrationBuilder.DropTable(
                name: "license_types");

            migrationBuilder.DropTable(
                name: "optical_form_text_locations");

            migrationBuilder.DropTable(
                name: "smses");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropTable(
                name: "templates");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "exam_booklet_type");

            migrationBuilder.DropTable(
                name: "answer_form_format");

            migrationBuilder.DropTable(
                name: "exam_types");

            migrationBuilder.DropTable(
                name: "score_formulas");

            migrationBuilder.DropTable(
                name: "form_lesson_sections");

            migrationBuilder.DropTable(
                name: "optical_form_definitions");

            migrationBuilder.DropTable(
                name: "units");

            migrationBuilder.DropTable(
                name: "districts");

            migrationBuilder.DropTable(
                name: "classrooms");

            migrationBuilder.DropTable(
                name: "formula_types");

            migrationBuilder.DropTable(
                name: "directions");

            migrationBuilder.DropTable(
                name: "optical_form_types");

            migrationBuilder.DropTable(
                name: "lessons");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "school_types");

            migrationBuilder.DropSequence(
                name: "appsettings_seq");

            migrationBuilder.DropSequence(
                name: "exam_type_optical_form_seq");

            migrationBuilder.DropSequence(
                name: "exam_types_seq");

            migrationBuilder.DropSequence(
                name: "exams_seq");

            migrationBuilder.DropSequence(
                name: "form_lesson_sections_seq");

            migrationBuilder.DropSequence(
                name: "lessons_seq");

            migrationBuilder.DropSequence(
                name: "optical_form_definitions_seq");

            migrationBuilder.DropSequence(
                name: "optical_form_text_locations_seq");

            migrationBuilder.DropSequence(
                name: "optical_form_types_seq");

            migrationBuilder.DropSequence(
                name: "sms_seq");

            migrationBuilder.DropSequence(
                name: "subjects_seq");

            migrationBuilder.DropSequence(
                name: "templates_seq");

            migrationBuilder.DropSequence(
                name: "units_seq");

            migrationBuilder.DropSequence(
                name: "user_seq");
        }
    }
}
