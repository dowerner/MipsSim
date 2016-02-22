CC=mips-elf-gcc
LD=mips-elf-ld
AS=mips-elf-as

CFLAGS+=-g
LDFLAGS=-T idt.ld -static 

all: skalar

skalar:  skalar.S skalar-main.c
	$(CC) $(CFLAGS) -c skalar.S
	$(CC) $(CFLAGS) -c skalar-main.c
	$(CC) $(LDFLAGS) skalar-main.o skalar.o -o skalar

clean:
	rm -f skalar skalar.o skalar-main.o




