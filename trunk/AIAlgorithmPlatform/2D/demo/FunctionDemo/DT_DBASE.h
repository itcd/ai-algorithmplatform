#ifndef _DT_DBASE_H_INCLUDED
#define _DT_DBASE_H_INCLUDED

#include <stdio.h>

#ifdef	 USE_DBMALLOC
#include <dbmalloc.h>
#endif

#ifdef __cplusplus
extern "C" {
#endif
    
#define TRIM_DBF_WHITESPACE

/************************************************************************/
typedef	struct
{
    FILE	*fp;
    int     nRecords;
    int		nRecordLength;
    int		nHeaderLength;
    int		nFields;
    int		*panFieldOffset;
    int		*panFieldSize;
    int		*panFieldDecimals;
    char	*pachFieldType;
    char	*pszHeader;
    int		nCurrentRecord;
    int		bCurrentRecordModified;
    char	*pszCurrentRecord;
    int		bNoHeader;
    int		bUpdated;
} DBFInfo;
typedef DBFInfo * DBFHandle;

typedef enum {
  FTString,
  FTInteger,
  FTDouble,
  FTInvalid
} DBFFieldType;

#define XBASE_FLDHDR_SZ       32

DBFHandle	 DBFOpen(const char* pszDBFFile,const char* pszAccess);
DBFHandle	 DBFCreate(const char* pszDBFFile);
int			 DBFGetFieldCount(DBFHandle psDBF);
int			 DBFGetRecordCount(DBFHandle psDBF);
int			 DBFAddField(DBFHandle hDBF,const char* pszFieldName,DBFFieldType eType,int nWidth,int nDecimals);
DBFFieldType DBFGetFieldInfo(DBFHandle psDBF,int iField,char* pszFieldName,int* pnWidth,int* pnDecimals);
int 		 DBFReadIntegerAttribute(DBFHandle hDBF,int iRecord,int iField );
double 	     DBFReadDoubleAttribute(DBFHandle hDBF,int iRecord,int iField );
const char*  DBFReadStringAttribute(DBFHandle hDBF,int iRecord,int iField );
int			 DBFWriteIntegerAttribute(DBFHandle hDBF,int iRecord,int iField,int nFieldValue );
int			 DBFWriteDoubleAttribute(DBFHandle hDBF,int iRecord,int iField,double dFieldValue );
int			 DBFWriteStringAttribute(DBFHandle hDBF,int iRecord,int iField,const char * pszFieldValue );
const char*  DBFReadTuple(DBFHandle psDBF,int hEntity );
int			 DBFWriteTuple(DBFHandle psDBF,int hEntity,void* pRawTuple );
DBFHandle    DBFCloneEmpty(DBFHandle psDBF,const char* pszFilename );
int          DBFGetFieldIndex(DBFHandle hDBF,char* pszFieldName);
void		 DBFClose( DBFHandle hDBF );

#ifdef __cplusplus
}
#endif

#endif 
