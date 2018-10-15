# coding: utf-8
from django.db import models

class User(models.Model):
    name   = models.CharField    (default  = "foge", max_length = 10)
    block  = models.IntegerField (default  = 0,      help_text  = "破壊したブロック数の合計")