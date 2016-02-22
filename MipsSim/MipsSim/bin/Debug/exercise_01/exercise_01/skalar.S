/* -*- asm -*-
 *
 * MIPS assembler funktion die das skalarprodukt zweier vektoren berechnet
 * C-interface: extern skalar(int* a, int* b, int n);
 *
 */

#include "regdef.h"

	.globl	skalar
	.ent	skalar,0
skalar:
	.frame	sp,0,ra

	/* pre-condition:
	 * a0 enthaelt adresse von vektor A
	 * a1 enthaelt adresse von vektor B
	 * a2 enthaelt die anzahl der elemente in den vektoren
	 */


	/* dieser code muss mit der loesung ersetzt werden: */
	li v0,999
	
	/* post-condition:
	 * das resultat des skalarproduktes ist in register v0 gespeichert 
	 */

	j	ra
	.end	skalar
  
