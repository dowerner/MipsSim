#include <stdio.h>

#define N 20

static int A[N];
static int B[N];

/* 
 * Diese Funktion scalar ist hier nur deklariert.
 * Die Implementation erfolgt in MIPS assembler im file skalar.S
 *
 * Das korrekte Skalarprodukt ist 9880
 */
extern skalar(int* a, int* b, int n);

void fill(int* a, int n)
{
    int i;

    for (i = 0; i < n; i++) {
    	a[i] = i*2;
    }
}

int main(void) {
    int result;
    
    fill(A, N);	
    fill(B, N);	
    result = skalar(A, B, N);
    printf("\nBerechne Skalarprodukt von A und B.\n");
    printf("   Ergebins erwartet:  9880\n   Ergebnis errechnet: %d\n", result);
    return 0;
}
