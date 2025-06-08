using apiFestivos.Aplicacion.Servicios;
using apiFestivos.Core.Interfaces.Repositorios;
using apiFestivos.Core.Interfaces.Servicios;
using apiFestivos.Dominio.Entidades;
using Moq;


namespace apiFestivos.Test
{
    public class FestivoServicioTest
    {
        private readonly IFestivoServicio festivoServicio;
        private readonly Mock<IFestivoRepositorio> festivoRepositorioMock;

        public FestivoServicioTest()
        {
            festivoRepositorioMock = new Mock<IFestivoRepositorio>();
            festivoServicio = new FestivoServicio(festivoRepositorioMock.Object);
        }

        [Fact]
        public async Task EsFestivo_FechaCoincideConFestivo_RetornaTrue()
        {
            // Arrange
            var fecha = new DateTime(2025, 1, 1);
            var festivo = new Festivo { IdTipo = 1, Dia = 1, Mes = 1, Nombre = "Año nuevo" };


            festivoRepositorioMock.Setup(r => r.ObtenerTodos())
                .ReturnsAsync(new List<Festivo> { festivo });

            // Act
            var resultado = await festivoServicio.EsFestivo(fecha);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public async Task EsFestivo_FechaNoCoincideConFestivo_RetornaFalse()
        {
            // Arrange
            var fecha = new DateTime(2025, 2, 2); 
            var festivo = new Festivo { IdTipo = 1, Dia = 1, Mes = 1, Nombre = "Año nuevo" };

            festivoRepositorioMock.Setup(r => r.ObtenerTodos())
                .ReturnsAsync(new List<Festivo> { festivo });

            // Act
            var resultado = await festivoServicio.EsFestivo(fecha);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task ObtenerFestivo_Tipo1_RetornaFechaEsperada()
        {
            // Arrange
            var año = 2026;
            var festivo = new Festivo
            { IdTipo = 1, Dia = 25, Mes = 12, Nombre = "Navidad" };

            var festivoRepositorioMock = new Mock<IFestivoRepositorio>();
            festivoRepositorioMock.Setup(r => r.ObtenerTodos())
                .ReturnsAsync(new List<Festivo> { festivo });

            var festivoServicio = new FestivoServicio(festivoRepositorioMock.Object);

            // Act
            var resultado = await festivoServicio.ObtenerAño(año);

            // Assert
            var festivoCalculado = resultado.FirstOrDefault();
            Assert.NotNull(festivoCalculado);
            Assert.Equal(new DateTime(2026, 12, 25), festivoCalculado.Fecha);
            Assert.Equal("Navidad", festivoCalculado.Nombre); 
        }

        [Fact]
        public async Task ObtenerFestivo_Tipo2_RetornaLunesSiguiente()
        {
            // Arrange
            var año = 2026;
            var festivo = new Festivo
            { IdTipo = 2, Dia = 20, Mes = 7, Nombre = "Independencia Colombia"};

            festivoRepositorioMock.Setup(r => r.ObtenerTodos())
                .ReturnsAsync(new List<Festivo> { festivo });

            // Act
            var resultado = await festivoServicio.ObtenerAño(año);

            var festivoCalculado = resultado.FirstOrDefault();

            // Assert
            Assert.NotNull(festivoCalculado);
            Assert.Equal("Independencia Colombia", festivoCalculado.Nombre);
            Assert.Equal(new DateTime(2026, 7, 20), festivoCalculado.Fecha);
        }

        [Fact]
        public async Task ObtenerFestivo_Tipo4_SeDesplazaAlLunesSiguiente()
        {
            // Arrange
            var año = 2026;
            var festivo = new Festivo
            {
                IdTipo = 4,
                DiasPascua = 39,
                Nombre = "Ascensión del Señor"
            };

            festivoRepositorioMock.Setup(r => r.ObtenerTodos())
                .ReturnsAsync(new List<Festivo> { festivo });

            // Act
            var resultado = await festivoServicio.ObtenerAño(año);
            var festivoCalculado = resultado.FirstOrDefault();

            // Assert
            Assert.NotNull(festivoCalculado);
            Assert.Equal("Ascensión del Señor", festivoCalculado.Nombre);
            Assert.Equal(new DateTime(2026, 5, 18), festivoCalculado.Fecha);
        }
    }
}
