
static void matrix(int *a, int *b, int *c, int n, int m) {
/* multiply Matrix a(n x m), b(m x n), n Zeilen, m Spalten, result in c */
	int i,j,k,sum;
	
	for(i=0; i < n; i++) {/* alle Zeilen */
		for(j = 0; j < n; j++) { /* alle Spalten */
			sum= 0;
			for(k = 0; k < m; k++) { /* Skalarprodukt */
				sum = sum + (a[i * m + k] * b[k * n + j]);
			}
			c[i * n + j] = sum;	
		}
	}
}

