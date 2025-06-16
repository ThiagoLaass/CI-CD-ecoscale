using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EcoScale.Migrations
{
    /// <inheritdoc />
    public partial class fix_relatorio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "email_confirmacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    num_codigo = table.Column<string>(type: "text", nullable: false),
                    boo_confirmado = table.Column<bool>(type: "boolean", nullable: false),
                    boo_excluido = table.Column<bool>(type: "boolean", nullable: false),
                    ref_id = table.Column<string>(type: "text", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_confirmacao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "notificacao",
                columns: table => new
                {
                    notificacao_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mensagem = table.Column<string>(type: "text", nullable: false),
                    boo_lida = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notificacao", x => x.notificacao_id);
                });

            migrationBuilder.CreateTable(
                name: "planilha",
                columns: table => new
                {
                    planilha_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    boo_excluido = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_planilha", x => x.planilha_id);
                });

            migrationBuilder.CreateTable(
                name: "recomendacoes",
                columns: table => new
                {
                    recomendacoes_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    arr_projetos_1_ano = table.Column<string[]>(type: "text[]", nullable: false),
                    arr_quick_wins_90d = table.Column<string[]>(type: "text[]", nullable: false),
                    arr_transformacoes_estrategicas = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recomendacoes", x => x.recomendacoes_id);
                });

            migrationBuilder.CreateTable(
                name: "responsavel_empresa",
                columns: table => new
                {
                    responsavel_empresa_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "text", nullable: false),
                    telefone = table.Column<string>(type: "text", nullable: false),
                    cpf = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_responsavel_empresa", x => x.responsavel_empresa_id);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "text", nullable: false),
                    senha = table.Column<string>(type: "text", nullable: false),
                    boo_email_confirmado = table.Column<bool>(type: "boolean", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.usuario_id);
                });

            migrationBuilder.CreateTable(
                name: "area_planilha",
                columns: table => new
                {
                    area_planilha_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_area = table.Column<string>(type: "text", nullable: false),
                    planilha_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_area_planilha", x => x.area_planilha_id);
                    table.ForeignKey(
                        name: "FK_area_planilha_planilha_planilha_id",
                        column: x => x.planilha_id,
                        principalTable: "planilha",
                        principalColumn: "planilha_id");
                });

            migrationBuilder.CreateTable(
                name: "empresa",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "integer", nullable: false),
                    cnpj = table.Column<string>(type: "text", nullable: false),
                    razao_social = table.Column<string>(type: "text", nullable: false),
                    num_telefone = table.Column<string>(type: "text", nullable: false),
                    endereco_sede = table.Column<string>(type: "text", nullable: false),
                    foto_perfil = table.Column<byte[]>(type: "bytea", nullable: true),
                    foto_perfil_mime = table.Column<string>(type: "text", nullable: true),
                    dsc_empresa = table.Column<string>(type: "text", nullable: true),
                    dsc_contexto = table.Column<string>(type: "text", nullable: true),
                    setor_atuacao = table.Column<string>(type: "text", nullable: true),
                    boo_removida = table.Column<bool>(type: "boolean", nullable: false),
                    responsavel_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empresa", x => x.usuario_id);
                    table.ForeignKey(
                        name: "FK_empresa_responsavel_empresa_responsavel_id",
                        column: x => x.responsavel_id,
                        principalTable: "responsavel_empresa",
                        principalColumn: "responsavel_empresa_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_empresa_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "moderador",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_moderador", x => x.usuario_id);
                    table.ForeignKey(
                        name: "FK_moderador_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tema_planilha",
                columns: table => new
                {
                    tema_planilha_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_tema = table.Column<string>(type: "text", nullable: false),
                    area_planilha_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tema_planilha", x => x.tema_planilha_id);
                    table.ForeignKey(
                        name: "FK_tema_planilha_area_planilha_area_planilha_id",
                        column: x => x.area_planilha_id,
                        principalTable: "area_planilha",
                        principalColumn: "area_planilha_id");
                });

            migrationBuilder.CreateTable(
                name: "questionario",
                columns: table => new
                {
                    questionario_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    boo_excluido = table.Column<bool>(type: "boolean", nullable: false),
                    empresa_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questionario", x => x.questionario_id);
                    table.ForeignKey(
                        name: "FK_questionario_empresa_empresa_id",
                        column: x => x.empresa_id,
                        principalTable: "empresa",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "relatorio",
                columns: table => new
                {
                    relatorio_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    empresa_cnpj = table.Column<int>(type: "integer", nullable: false),
                    RevisorId = table.Column<int>(type: "integer", nullable: true),
                    boo_revisado = table.Column<bool>(type: "boolean", nullable: false),
                    nota = table.Column<double>(type: "double precision", nullable: false),
                    dsc_diagnostico = table.Column<string>(type: "text", nullable: false),
                    arr_pontos_criticos = table.Column<string[]>(type: "text[]", nullable: false),
                    arr_pontos_fortes = table.Column<string[]>(type: "text[]", nullable: false),
                    recomendacoes_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_relatorio", x => x.relatorio_id);
                    table.ForeignKey(
                        name: "FK_relatorio_empresa_empresa_cnpj",
                        column: x => x.empresa_cnpj,
                        principalTable: "empresa",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_relatorio_moderador_RevisorId",
                        column: x => x.RevisorId,
                        principalTable: "moderador",
                        principalColumn: "usuario_id");
                    table.ForeignKey(
                        name: "FK_relatorio_recomendacoes_recomendacoes_id",
                        column: x => x.recomendacoes_id,
                        principalTable: "recomendacoes",
                        principalColumn: "recomendacoes_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "criterio_planilha",
                columns: table => new
                {
                    criterio_planilha_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_criterio = table.Column<string>(type: "text", nullable: false),
                    tema_planilha_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_criterio_planilha", x => x.criterio_planilha_id);
                    table.ForeignKey(
                        name: "FK_criterio_planilha_tema_planilha_tema_planilha_id",
                        column: x => x.tema_planilha_id,
                        principalTable: "tema_planilha",
                        principalColumn: "tema_planilha_id");
                });

            migrationBuilder.CreateTable(
                name: "area",
                columns: table => new
                {
                    area_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_area = table.Column<string>(type: "text", nullable: false),
                    questionario_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_area", x => x.area_id);
                    table.ForeignKey(
                        name: "FK_area_questionario_questionario_id",
                        column: x => x.questionario_id,
                        principalTable: "questionario",
                        principalColumn: "questionario_id");
                });

            migrationBuilder.CreateTable(
                name: "req_avaliacao",
                columns: table => new
                {
                    req_avaliacao_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    relatorio_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    em_aberto = table.Column<bool>(type: "boolean", nullable: false),
                    avaliado = table.Column<bool>(type: "boolean", nullable: false),
                    motivo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_req_avaliacao", x => x.req_avaliacao_id);
                    table.ForeignKey(
                        name: "FK_req_avaliacao_relatorio_relatorio_id",
                        column: x => x.relatorio_id,
                        principalTable: "relatorio",
                        principalColumn: "relatorio_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_avaliado_planilha",
                columns: table => new
                {
                    item_avaliado_planilha_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dsc_item = table.Column<string>(type: "text", nullable: false),
                    criterio_planilha_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_avaliado_planilha", x => x.item_avaliado_planilha_id);
                    table.ForeignKey(
                        name: "FK_item_avaliado_planilha_criterio_planilha_criterio_planilha_~",
                        column: x => x.criterio_planilha_id,
                        principalTable: "criterio_planilha",
                        principalColumn: "criterio_planilha_id");
                });

            migrationBuilder.CreateTable(
                name: "tema",
                columns: table => new
                {
                    tema_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_tema = table.Column<string>(type: "text", nullable: false),
                    area_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tema", x => x.tema_id);
                    table.ForeignKey(
                        name: "FK_tema_area_area_id",
                        column: x => x.area_id,
                        principalTable: "area",
                        principalColumn: "area_id");
                });

            migrationBuilder.CreateTable(
                name: "criterio",
                columns: table => new
                {
                    criterio_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom_criterio = table.Column<string>(type: "text", nullable: false),
                    tema_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_criterio", x => x.criterio_id);
                    table.ForeignKey(
                        name: "FK_criterio_tema_tema_id",
                        column: x => x.tema_id,
                        principalTable: "tema",
                        principalColumn: "tema_id");
                });

            migrationBuilder.CreateTable(
                name: "item_avaliado",
                columns: table => new
                {
                    item_avaliado_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dsc_item = table.Column<string>(type: "text", nullable: false),
                    resposta = table.Column<string>(type: "text", nullable: true),
                    criterio_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_avaliado", x => x.item_avaliado_id);
                    table.ForeignKey(
                        name: "FK_item_avaliado_criterio_criterio_id",
                        column: x => x.criterio_id,
                        principalTable: "criterio",
                        principalColumn: "criterio_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_area_questionario_id",
                table: "area",
                column: "questionario_id");

            migrationBuilder.CreateIndex(
                name: "IX_area_planilha_planilha_id",
                table: "area_planilha",
                column: "planilha_id");

            migrationBuilder.CreateIndex(
                name: "IX_criterio_tema_id",
                table: "criterio",
                column: "tema_id");

            migrationBuilder.CreateIndex(
                name: "IX_criterio_planilha_tema_planilha_id",
                table: "criterio_planilha",
                column: "tema_planilha_id");

            migrationBuilder.CreateIndex(
                name: "IX_empresa_responsavel_id",
                table: "empresa",
                column: "responsavel_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_avaliado_criterio_id",
                table: "item_avaliado",
                column: "criterio_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_avaliado_planilha_criterio_planilha_id",
                table: "item_avaliado_planilha",
                column: "criterio_planilha_id");

            migrationBuilder.CreateIndex(
                name: "IX_questionario_empresa_id",
                table: "questionario",
                column: "empresa_id");

            migrationBuilder.CreateIndex(
                name: "IX_relatorio_empresa_cnpj",
                table: "relatorio",
                column: "empresa_cnpj");

            migrationBuilder.CreateIndex(
                name: "IX_relatorio_recomendacoes_id",
                table: "relatorio",
                column: "recomendacoes_id");

            migrationBuilder.CreateIndex(
                name: "IX_relatorio_RevisorId",
                table: "relatorio",
                column: "RevisorId");

            migrationBuilder.CreateIndex(
                name: "IX_req_avaliacao_relatorio_id",
                table: "req_avaliacao",
                column: "relatorio_id");

            migrationBuilder.CreateIndex(
                name: "IX_tema_area_id",
                table: "tema",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "IX_tema_planilha_area_planilha_id",
                table: "tema_planilha",
                column: "area_planilha_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_email",
                table: "usuario",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "email_confirmacao");

            migrationBuilder.DropTable(
                name: "item_avaliado");

            migrationBuilder.DropTable(
                name: "item_avaliado_planilha");

            migrationBuilder.DropTable(
                name: "notificacao");

            migrationBuilder.DropTable(
                name: "req_avaliacao");

            migrationBuilder.DropTable(
                name: "criterio");

            migrationBuilder.DropTable(
                name: "criterio_planilha");

            migrationBuilder.DropTable(
                name: "relatorio");

            migrationBuilder.DropTable(
                name: "tema");

            migrationBuilder.DropTable(
                name: "tema_planilha");

            migrationBuilder.DropTable(
                name: "moderador");

            migrationBuilder.DropTable(
                name: "recomendacoes");

            migrationBuilder.DropTable(
                name: "area");

            migrationBuilder.DropTable(
                name: "area_planilha");

            migrationBuilder.DropTable(
                name: "questionario");

            migrationBuilder.DropTable(
                name: "planilha");

            migrationBuilder.DropTable(
                name: "empresa");

            migrationBuilder.DropTable(
                name: "responsavel_empresa");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
