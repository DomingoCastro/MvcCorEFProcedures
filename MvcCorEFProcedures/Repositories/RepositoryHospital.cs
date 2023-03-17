using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCorEFProcedures.Data;
using MvcCorEFProcedures.Models;

#region SQL SERVER PROCEDURES
/*
CREATE PROCEDURE SP_GET_ENFERMOS
AS
	select * from ENFERMO
GO


CREATE PROCEDURE SP_FIND_ENFERMO(@INSCRIPCION NVARCHAR(30))
as
    SELECT* FROM ENFERMO WHERE INSCRIPCION = @INSCRIPCION
go


CREATE PROCEDURE SP_DELETE_ENFERMO(@INSCRIPCION NVARCHAR(30))
AS
    DELETE FROM ENFERMO WHERE INSCRIPCION = @INSCRIPCION
GO


CREATE VIEW V_PAGINAR_EMPLEADOS
AS
SELECT CAST(
ROW_NUMBER() OVER (ORDER BY APELLIDO) AS int)
AS POSICION,
EMP.EMP_NO, EMP.APELLIDO, EMP.SALARIO, EMP.OFICIO, EMP.DEPT_NO FROM EMP
GO
-- NECESITAMOS PAGINAR DE 3 EN 3
CREATE PROCEDURE SP_PAGINAR_EMPLEADOS(@POSICION INT)
AS
SELECT EMP_NO, APELLIDO, SALARIO, OFICIO, DEPT_NO FROM V_PAGINAR_EMPLEADOS WHERE POSICION >= @POSICION AND POSICION < (@POSICION+ 3)
GO
EXEC SP_PAGINAR_EMPLEADOS 1

*/
#endregion



namespace MvcCorEFProcedures.Repositories
{
    public class RepositoryHospital
    {
        private HospitalContext context;

        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }

        public List<Enfermo> GetEnfermos()
        {
            string sql = "SP_GET_ENFERMOS";
            //PRIMERO LA CONSULTA Y DESPUES EXTRAER
            var consulta = this.context.Enfermos.FromSqlRaw(sql);
            //EXTRAEMOS LOS DATOS
            List<Enfermo> enfermos =
                consulta.AsEnumerable().ToList();
            return enfermos;
            
        }
        //PROCEDIMIENTOS CON PARAMETROS
        public Enfermo FindEnfermos(string inscripcion)
        {
            //LOS PARAMETROS SE SEPARAN CON UN ESPACIO DEL PROCEDIMIENTO ENTRE COMAS
            // SP_PROCEMIENTO @PARAM1, @PARAM2, @PARAM3 ....
            string sql = "SP_FIND_ENFERMO @INSCRIPCION";
            SqlParameter paminscrip = new SqlParameter("@INSCRIPCION", inscripcion);
            //SI TUVIERAMOS VARIOS PARAMETROS
            //VAR CONSULTA = THIS.CONTEXT.ENTITY.FROMSQLRAW(SQL, PAM1, PAM2, PAM3)
            var consulta = this.context.Enfermos.FromSqlRaw(sql, paminscrip);
            Enfermo enfermo= consulta.AsEnumerable().FirstOrDefault();
            return enfermo;
        }
        public void DeleteEnfermo(string inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO @INSCRIPCION";
            SqlParameter paminscripcion = new SqlParameter("@INSCRIPCION", inscripcion);
            this.context.Database.ExecuteSqlRaw(sql, paminscripcion);
        }

        public int GetNumeroEmpleados()
        {
            return this.context.Empleado.Count();
        }

        public async Task<List<Empleado>> GetEmpleadosAsync(int POSICION) 
        {
            string sql = "SP_PAGINAR_EMPLEADOS @POSICION";
            SqlParameter pamempleados = new SqlParameter("@POSICION", POSICION);
            var consulta = this.context.Empleado.FromSqlRaw(sql, pamempleados);
            List<Empleado> empleados = await consulta.ToListAsync();
            return empleados;
        }
    }
}
